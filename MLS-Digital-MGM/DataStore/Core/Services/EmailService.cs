using DataStore.Core.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using DataStore.Core.Models;
using DataStore.Persistence.Interfaces;
using Hangfire;
using System.Text.Json;
using DataStore.Core.DTOs.User;
using DataStore.Helpers;
using AutoMapper;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace DataStore.Core.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration { get; }
        private readonly IErrorLogService _errorService;
        private readonly AsyncRetryPolicy _retryPolicy;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(5);
        private const int DailyEmailLimit = 1000;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly bool _useMailtrap;
        private readonly bool _enableEmailSending;
        private readonly SignatureService _signatureService;
        // Add to class fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public EmailService(
            IConfiguration configuration, 
            IErrorLogService errorService, 
            IRepositoryManager repositoryManager,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            SignatureService signatureService,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
           )
        {
            _configuration = configuration;
            _errorService = errorService;
            _repositoryManager = repositoryManager;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
            _useMailtrap = _configuration.GetValue<bool>("MailSettings:UseMailtrap");
            _enableEmailSending = _configuration.GetValue<bool>("MailSettings:EnableEmailSending");
            _signatureService = signatureService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _retryPolicy = Policy
                .Handle<SmtpCommandException>()
                .Or<SmtpProtocolException>()
                .Or<IOException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _errorService.LogErrorAsync(new Exception($"Retry {retryCount} for sending email to {context["email"]}. Waiting {timeSpan} before next retry. Error: {exception.Message}"));
                    });
        }
        
        public async Task<KeyValuePair<bool, string>> SendMailWithKeyVarReturn(string email, string subject, string htmlMessage, bool isFromQueue = false)
        {
            if (!_enableEmailSending)
            {
                await _errorService.LogErrorAsync(new Exception($"Email sending is disabled. Would have sent email to {email} with subject: {subject}"));
                return new KeyValuePair<bool, string>(true, "Email sending is disabled, but operation logged as successful");
            }

            await _semaphore.WaitAsync();
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(GetSetting("SenderName"), GetSetting("SenderEmail")));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;

                message.Body = new TextPart("html")
                {
                    Text = htmlMessage
                };

                var context = new Context();
                context["email"] = email;

                await _retryPolicy.ExecuteAsync(async (ctx) =>
                {
                    using var client = new SmtpClient();
                    client.Timeout = 30000; // 30 seconds timeout
                    await client.ConnectAsync(
                        GetSetting("Server"),
                        int.Parse(GetSetting("Port")),
                        SecureSocketOptions.SslOnConnect
                    );
                    await client.AuthenticateAsync(
                        GetSetting("SenderEmail"),
                        GetSetting("Password")
                    );
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }, context);

                return new KeyValuePair<bool, string>(true, "Message sent");
            }
            catch (SmtpCommandException ex) when (ex.StatusCode == SmtpStatusCode.MailboxUnavailable && ex.Message.Contains("Daily user sending limit exceeded"))
            {
                await _errorService.LogErrorAsync(ex);
                if (!isFromQueue)
                {
                    await QueueEmailAsync(email, subject, htmlMessage, "DailyLimitExceeded");
                    return new KeyValuePair<bool, string>(false, "Daily sending limit exceeded. Email queued for later sending.");
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, "Daily sending limit exceeded. Email remains in queue.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Message not sent. Error: {ex.Message}";
                await _errorService.LogErrorAsync(ex);
                return new KeyValuePair<bool, string>(false, errorMessage);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private string GetSetting(string key)
        {
            string prefix = _useMailtrap ? "MailSettings:Mailtrap:" : "MailSettings:Gmail:";
            return _configuration[prefix + key];
        }

        public async Task QueueEmailAsync(string email, string subject, string body, string emailType)
        {
            var emailQueue = new EmailQueue
            {
                RecipientEmail = email,
                Subject = subject,
                Body = body,
                ScheduledDate = await GetNextAvailableSendDate(),
                IsSent = false,
                EmailType = emailType
            };

            await _repositoryManager.EmailQueueRepository.AddAsync(emailQueue);
            await _repositoryManager.UnitOfWork.SaveChangesAsync();

            if (!_enableEmailSending)
            {
                await _errorService.LogErrorAsync(new Exception($"Email sending is disabled. Email queued but will not be sent: To: {email}, Subject: {subject}"));
            }
        }

        private async Task<DateTime> GetNextAvailableSendDate()
        {
            var today = DateTime.Today;
            var emailsSentToday = await _repositoryManager.EmailQueueRepository.GetEmailQueueCountAsync(e => e.ScheduledDate == today);

            if (emailsSentToday < DailyEmailLimit)
            {
                return today;
            }

            // Find the next day with available slots
            var nextDate = today.AddDays(1);
            while (await _repositoryManager.EmailQueueRepository.GetEmailQueueCountAsync(e => e.ScheduledDate == nextDate) >= DailyEmailLimit)
            {
                nextDate = nextDate.AddDays(1);
            }

            return nextDate;
        }

        public async Task ProcessEmailQueueAsync()
        {
            if (!_enableEmailSending)
            {
                await _errorService.LogErrorAsync(new Exception("Email sending is disabled. Email queue processing skipped."));
                return;
            }

            var today = DateTime.Today;
            var emailsToSend = await _repositoryManager.EmailQueueRepository.GetEmailsToSendAsync(
                e => e.ScheduledDate <= today && !e.IsSent, 
                DailyEmailLimit
            );

            foreach (var email in emailsToSend)
            {
                var result = await SendMailWithKeyVarReturn(email.RecipientEmail, email.Subject, email.Body, isFromQueue: true);
                if (result.Key)
                {
                    email.IsSent = true;
                    email.SentDate = DateTime.Now;
                    await _repositoryManager.EmailQueueRepository.UpdateAsync(email);
                }
                else if (result.Value.Contains("Daily sending limit exceeded"))
                {
                    // If we hit the limit while processing the queue, stop processing for today
                    await _errorService.LogErrorAsync(new Exception("Daily sending limit reached while processing email queue. Stopping processing for today."));
                    break;
                }
            }

            await _repositoryManager.UnitOfWork.SaveChangesAsync();
        }

        public async Task SendCPDStatusEmailsAsync(List<string> memberEmails, string emailBody, string subject)
        {
            foreach (var memberEmail in memberEmails)
            {
                await this.SendMailWithKeyVarReturn(memberEmail, subject, emailBody);
            }
        }

        public async Task SendCPDInvoiceStatusEmailAsync(List<string> memberEmails, string emailBody, string subject)
        {
            foreach (var memberEmail in memberEmails)
            {
                await this.SendMailWithKeyVarReturn(memberEmail, subject, emailBody);
            }
        }

        public Task LogErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void ScheduleDailyEmailProcessing()
        {
            _recurringJobManager.AddOrUpdate(
                "ProcessDailyEmails",
                () => ProcessEmailQueueAsync(),
                Cron.Daily(2, 0) // Run at 2 AM every day
            );
        }

        public void EnqueueEmailSending(string email, string subject, string body)
        {
            _backgroundJobClient.Enqueue(() => QueueEmailAsync(email, subject, body, "Immediate"));

            if (!_enableEmailSending)
            {
                _errorService.LogErrorAsync(new Exception($"Email sending is disabled. Email enqueued but will not be sent: To: {email}, Subject: {subject}"));
            }
        }

        //communication message with or without attachments
        public async Task<KeyValuePair<bool, string>> SendMailFromCommunicationMessage(
            string email, 
            CommunicationMessage message, 
            string senderUserId = null, 
            bool includeSignature = true,
            bool isFromQueue = false)
        {
            if (!_enableEmailSending)
            {
                await _errorService.LogErrorAsync(new Exception($"Email sending is disabled. Would have sent email to {email} with subject: {message.Subject} and {message.Attachments?.Count ?? 0} attachment(s)"));
                return new KeyValuePair<bool, string>(true, "Email sending is disabled, but operation logged as successful");
            }

            await _semaphore.WaitAsync();
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(GetSetting("SenderName"), GetSetting("SenderEmail")));
                mimeMessage.To.Add(MailboxAddress.Parse(email));
                mimeMessage.Subject = message.Subject;

                var bodyBuilder = new BodyBuilder();
                
                // Add signature logic
                string emailBody = message.Body.Replace("\r\n", "<br>").Replace("\n", "<br>");
                if (includeSignature)
                {
                    if (!string.IsNullOrEmpty(senderUserId))
                    {
                        var sender = await _repositoryManager.UserRepository.GetSingleUser(senderUserId);
                        if (!string.IsNullOrEmpty(sender?.SignatureData))
                        {
                            var signatureData = JsonSerializer.Deserialize<SignatureDTO>(sender.SignatureData);
                            
                            // Handle personal signature banner from SignatureData
                            if (!string.IsNullOrEmpty(signatureData.BannerImageUrl))
                            {
                                var filePath = signatureData.BannerImageUrl.TrimStart('/');
                                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
                                
                                if (File.Exists(fullPath))
                                {
                                    var contentId = $"personal_banner_{Guid.NewGuid().ToString("N")}";
                                    var image = bodyBuilder.LinkedResources.Add(fullPath);
                                    image.ContentId = contentId;
                                    signatureData.BannerImageUrl = $"cid:{contentId}";
                                }
                                else
                                {
                                    await _errorService.LogErrorAsync(new Exception($"Banner image not found at path: {fullPath}"));
                                }
                            }
                            
                            emailBody += $"<br/><br/>{_signatureService.GenerateSignatureHtml(signatureData)}";
                        }
                    }
                }
                else
                {
                    // Check for active generic signature when personal signature is not included
                    var activeGenericSignature = await _repositoryManager.GenericSignatureRepository.GetSingleAsync(
                        s => s.IsActive == true && s.Status != Lambda.Deleted, 
                        new Expression<Func<GenericSignature, object>>[] { s => s.Attachments }
                    );
                    
                    if (activeGenericSignature != null)
                    {
                        var signatureData = _mapper.Map<SignatureDTO>(activeGenericSignature);
                        var bannerAttachment = activeGenericSignature.Attachments?
                            .FirstOrDefault(a => a.PropertyName == "Banner" && a.Status != Lambda.Deleted);
                        
                        if (bannerAttachment != null && File.Exists(bannerAttachment.FilePath))
                        {
                            var contentId = $"banner_{Guid.NewGuid().ToString("N")}";
                            var image = bodyBuilder.LinkedResources.Add(bannerAttachment.FilePath);
                            image.ContentId = contentId;
                            signatureData.BannerImageUrl = $"cid:{contentId}";
                        }

                        emailBody += $"<br/><br/>{_signatureService.GenerateSignatureHtml(signatureData)}";
                    }
                }

                bodyBuilder.HtmlBody = emailBody;

                if (message.Attachments != null && message.Attachments.Any())
                {
                    foreach (var attachment in message.Attachments)
                    {
                        if (File.Exists(attachment.FilePath))
                        {
                            using (var stream = File.OpenRead(attachment.FilePath))
                            {
                                var contentType = MimeTypes.GetMimeType(attachment.FileName);
                                bodyBuilder.Attachments.Add(attachment.FileName, stream, ContentType.Parse(contentType));
                            }
                        }
                        else
                        {
                            await _errorService.LogErrorAsync(new Exception($"Attachment file not found: {attachment.FilePath}"));
                        }
                    }
                }

                mimeMessage.Body = bodyBuilder.ToMessageBody();

                var context = new Context();
                context["email"] = email;

                await _retryPolicy.ExecuteAsync(async (ctx) =>
                {
                    using var client = new SmtpClient();
                    client.Timeout = 30000; // 30 seconds timeout
                    await client.ConnectAsync(
                        GetSetting("Server"),
                        int.Parse(GetSetting("Port")),
                        SecureSocketOptions.SslOnConnect
                    );
                    await client.AuthenticateAsync(
                        GetSetting("SenderEmail"),
                        GetSetting("Password")
                    );
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }, context);

                return new KeyValuePair<bool, string>(true, "Message sent successfully");
            }
            catch (SmtpCommandException ex) when (ex.StatusCode == SmtpStatusCode.MailboxUnavailable && ex.Message.Contains("Daily user sending limit exceeded"))
            {
                await _errorService.LogErrorAsync(ex);
                if (!isFromQueue)
                {
                    await QueueEmailFromCommunicationMessageAsync(email, message, "DailyLimitExceeded");
                    return new KeyValuePair<bool, string>(false, "Daily sending limit exceeded. Email queued for later sending.");
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, "Daily sending limit exceeded. Email remains in queue.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Message not sent. Error: {ex.Message}";
                await _errorService.LogErrorAsync(ex);
                return new KeyValuePair<bool, string>(false, errorMessage);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task QueueEmailFromCommunicationMessageAsync(string email, CommunicationMessage message, string emailType)
        {
            var emailQueue = new EmailQueue
            {
                RecipientEmail = email,
                Subject = message.Subject,
                Body = message.Body,
                ScheduledDate = await GetNextAvailableSendDate(),
                IsSent = false,
                EmailType = emailType,
                //Attachments = message.Attachments
            };

            await _repositoryManager.EmailQueueRepository.AddAsync(emailQueue);
            await _repositoryManager.UnitOfWork.SaveChangesAsync();

            if (!_enableEmailSending)
            {
                await _errorService.LogErrorAsync(new Exception($"Email sending is disabled. Email queued but will not be sent: To: {email}, Subject: {message.Subject}"));
            }
        }
    }
}
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

        public EmailService(
            IConfiguration configuration, 
            IErrorLogService errorService, 
            IRepositoryManager repositoryManager,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager)
        {
            _configuration = configuration;
            _errorService = errorService;
            _repositoryManager = repositoryManager;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
            _useMailtrap = _configuration.GetValue<bool>("MailSettings:UseMailtrap");
            _enableEmailSending = _configuration.GetValue<bool>("MailSettings:EnableEmailSending");

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
        
        public async Task<KeyValuePair<bool, string>> SendMailWithKeyVarReturn(string email, string subject, string htmlMessage)
        {
            if (!_enableEmailSending)
            {
                // Log that email sending is disabled
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

                var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
                message.Body = bodyBuilder.ToMessageBody();

                var context = new Context();
                context["email"] = email;

                await _retryPolicy.ExecuteAsync(async (ctx) =>
                {
                    using var client = new SmtpClient();
                    await client.ConnectAsync(
                        GetSetting("Server"),
                        int.Parse(GetSetting("Port")),
                        SecureSocketOptions.StartTls
                    );
                    await client.AuthenticateAsync(
                        GetSetting(_useMailtrap ? "UserName" : "SenderEmail"),
                        GetSetting("Password")
                    );
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }, context);

                return new KeyValuePair<bool, string>(true, "Message sent");
            }
            catch (Exception ex)
            {
                string errorMessage = "Message not sent";
                if (ex is SslHandshakeException || (ex is SocketException socketEx && socketEx.SocketErrorCode == SocketError.HostNotFound))
                {
                    errorMessage = "Message not sent due to internet-related issues. Please try again later.";
                }

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
                var result = await SendMailWithKeyVarReturn(email.RecipientEmail, email.Subject, email.Body);
                if (result.Key)
                {
                    email.IsSent = true;
                    email.SentDate = DateTime.Now;
                    await _repositoryManager.EmailQueueRepository.UpdateAsync(email);
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
    }
}
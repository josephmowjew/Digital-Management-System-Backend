using DataStore.Core.Services.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Services
{
    public class EmailService :  IEmailService
    {

        private IConfiguration _configuration { get; }

        private readonly IErrorLogService _errorService;


        public EmailService(IConfiguration configuration, IErrorLogService errorService)
        {
            _configuration = configuration;
            _errorService = errorService;
        }
        
        public async Task<KeyValuePair<bool, string>> SendMailWithKeyVarReturn(string email, string subject, string HtmlMessage)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress(_configuration["MailSettings:SenderName"], _configuration["MailSettings:SenderEmail"]);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(email, email);
                message.To.Add(to);

                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = HtmlMessage;

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.Auto);
                    await client.AuthenticateAsync(_configuration["MailSettings:SenderEmail"], _configuration["MailSettings:Password"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return new KeyValuePair<bool, string>(true, "Message sent");
            }
            catch (Exception ex)
            {
                string errorMessage = "Message not sent";
                if (ex is SslHandshakeException || (ex is SocketException socketEx && socketEx.SocketErrorCode == SocketError.HostNotFound))
                {
                    errorMessage = "Message not sent due to internet-related issues. Please try again later.";
                }

                await this._errorService.LogErrorAsync(ex);

                return new KeyValuePair<bool, string>(false, errorMessage);
            }
        }

         public async Task SendCPDStatusEmailsAsync(List<string> memberEmails,string emailBody,string subject)
        {
            foreach (var memberEmail in memberEmails)
            {
               
                await this.SendMailWithKeyVarReturn(subject, "CPD Attendance Status", emailBody);
            }
        }

        public Task LogErrorAsync(Exception ex)
        {
            throw new NotImplementedException();
        }


    }
}

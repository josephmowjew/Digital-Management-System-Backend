using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Services.Interfaces
{
    public interface IEmailService
    {
      
        Task SendCPDStatusEmailsAsync(List<string> memberEmails,string emailBody, string subject);

        Task SendCPDInvoiceStatusEmailAsync(List<string> memberEmails, string emailBody, string subject);
        Task ProcessEmailQueueAsync();
        Task QueueEmailAsync(string email, string subject, string body, string emailType);
        Task<KeyValuePair<bool, string>> SendMailWithKeyVarReturn(string email, string subject, string htmlMessage, bool isFromQueue = false);
    }
}

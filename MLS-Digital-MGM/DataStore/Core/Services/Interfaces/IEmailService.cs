using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Services.Interfaces
{
    public interface IEmailService
    {
        Task<KeyValuePair<bool, string>> SendMailWithKeyVarReturn(string email, string subject, string HtmlMessage);
        Task SendCPDStatusEmailsAsync(List<string> memberEmails,string emailBody, string subject);
    }
}

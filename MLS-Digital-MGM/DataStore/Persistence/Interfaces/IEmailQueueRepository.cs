using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IEmailQueueRepository : IRepository<EmailQueue>
    {
        Task<int> GetEmailQueueCountAsync();
        Task<int> GetEmailQueueCountAsync(Expression<Func<EmailQueue, bool>> filter);
        Task<List<EmailQueue>> GetEmailsToSendAsync(DateTime date, int limit);
        Task<List<EmailQueue>> GetEmailsToSendAsync(Expression<Func<EmailQueue, bool>> filter, int limit);
    }
}
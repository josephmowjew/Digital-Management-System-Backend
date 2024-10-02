using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class EmailQueueRepository : Repository<EmailQueue>, IEmailQueueRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public EmailQueueRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetEmailQueueCountAsync()
        {
            return await _context.EmailQueues
                .Where(eq => eq.Status == Lambda.Active)
                .CountAsync();
        }

        public async Task<int> GetEmailQueueCountAsync(Expression<Func<EmailQueue, bool>> filter)
        {
            return await _context.EmailQueues
                .Where(filter)
                .CountAsync();
        }

        public async Task<List<EmailQueue>> GetEmailsToSendAsync(DateTime date, int limit)
        {
            return await _context.EmailQueues
                .Where(e => e.ScheduledDate <= date && !e.IsSent)
                .OrderBy(e => e.ScheduledDate)
                .ThenBy(e => e.Id)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<EmailQueue>> GetEmailsToSendAsync(Expression<Func<EmailQueue, bool>> filter, int limit)
        {
            return await _context.EmailQueues
                .Where(filter)
                .OrderBy(e => e.ScheduledDate)
                .ThenBy(e => e.Id)
                .Take(limit)
                .ToListAsync();
        }
    }
}
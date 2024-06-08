using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thread = DataStore.Core.Models.Thread;

namespace DataStore.Persistence.SQLRepositories
{
    public class ThreadRepository : Repository<Thread>, IThreadRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public ThreadRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        // Implement additional methods here for thread-specific operations

        public async Task<IEnumerable<Thread>> GetAllAsync(int committeeId)
        {
            return await _context.Threads
                .Include(t => t.Messages).Include(t => t.CreatedBy).Where(q => q.CommitteeId == committeeId && q.Status != Lambda.Deleted)
                .ToListAsync();
        }
    }
}

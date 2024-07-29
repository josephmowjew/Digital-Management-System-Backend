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
    public class SubcommitteeThreadRepository : Repository<SubcommitteeThread>, ISubcommitteeThreadRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SubcommitteeThreadRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SubcommitteeThread>> GetAllAsync(int subcommitteeId)
        {
            return await _context.SubcommitteeThreads
                .Include(t => t.SubcommitteeMessages)
                .Include(t => t.CreatedBy)
                .Where(q => q.SubcommitteeId == subcommitteeId && q.Status != Lambda.Deleted)
                .ToListAsync();
        }
    }
}

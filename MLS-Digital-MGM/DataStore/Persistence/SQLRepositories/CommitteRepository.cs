using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using DataStore.Core.Models;

namespace DataStore.Persistence.SQLRepositories
{
    public class CommitteeRepository : Repository<Committee>, ICommitteeRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public CommitteeRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        // Implement additional methods here for committee-specific operations

        public async Task<int> GetCommitteeCount()
        {
            return await _context.Committees.CountAsync(c => c.Status == Lambda.Active);
        }
    }
}

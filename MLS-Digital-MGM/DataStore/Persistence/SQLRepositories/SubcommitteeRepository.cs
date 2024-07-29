using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using DataStore.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class SubcommitteeRepository : Repository<Subcommittee>, ISubcommitteeRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SubcommitteeRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<int> GetSubcommitteeCount()
        {
            return await _context.Subcommittees.CountAsync(s => s.Status == Lambda.Active);
        }

        /*public async Task<IEnumerable<Subcommittee>> GetSubcommitteesByCommitteeId(int committeeId)
        {
            return await _context.Subcommittees
                .Where(s => s.CommitteeId == committeeId && s.Status == Lambda.Active)
                .ToListAsync();
        }

        public async Task<Subcommittee> GetSubcommitteeWithChairperson(int subcommitteeId)
        {
            return await _context.Subcommittees
                .Include(s => s.Chairperson)
                .FirstOrDefaultAsync(s => s.Id == subcommitteeId && s.Status == Lambda.Active);
        }

        public async Task<IEnumerable<Subcommittee>> GetSubcommitteesWithMemberships()
        {
            return await _context.Subcommittees
                .Include(s => s.Memberships)
                .Where(s => s.Status == Lambda.Active)
                .ToListAsync();
        }*/
    }
}

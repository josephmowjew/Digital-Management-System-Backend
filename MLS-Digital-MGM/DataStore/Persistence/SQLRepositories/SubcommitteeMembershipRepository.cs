using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Persistence.SQLRepositories
{
    public class SubcommitteeMembershipRepository : Repository<SubcommitteeMembership>, ISubcommitteeMembershipRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SubcommitteeMembershipRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        // Implement additional methods here for subcommittee membership-specific operations

        public async Task<SubcommitteeMembership> GetByIdAsync(int subcommitteeMembershipId)
        {
            return await _context.SubcommitteeMemberships
                .Include(s => s.MemberShip)
                .Include(s => s.Subcommittee)
                .FirstOrDefaultAsync(s => s.Id == subcommitteeMembershipId);
        }

        public async Task<SubcommitteeMembership> GetSubcommitteeMembershipAsync(int subcommitteeId, string memberId)
        {
            return await _context.SubcommitteeMemberships
                .Include(s => s.MemberShip)
                .Include(s => s.Subcommittee)
                .FirstOrDefaultAsync(s => s.SubcommitteeID == subcommitteeId && s.MemberShipId == memberId);
        }
    }
}

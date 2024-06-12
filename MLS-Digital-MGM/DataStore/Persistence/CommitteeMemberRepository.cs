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
    public class CommitteeMemberRepository : Repository<CommitteeMembership>, ICommitteeMemberRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public CommitteeMemberRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        // Implement additional methods here for committee member-specific operations

        public async Task<CommitteeMembership> GetByIdAsync(int committeeMemberId)
        {
            return await _context.CommitteeMembers.Include(c => c.MemberShip).Include(c => c.Committee).FirstOrDefaultAsync(c => c.Id == committeeMemberId);
        }
        public async Task<CommitteeMembership> GetCommitteeMembershipAsync(int committeeId, string memberId)
        {
            return await _context.CommitteeMembers.Include(c => c.MemberShip).Include(c => c.Committee).FirstOrDefaultAsync(c => c.CommitteeID == committeeId && c.MemberShipId == memberId);
        }
    }
}

using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface ICommitteeMemberRepository : IRepository<CommitteeMembership>
    {
        // Additional methods specific to the CommitteeMember entity, if needed

        Task<CommitteeMembership> GetByIdAsync(int committeeMemberId);

        Task<CommitteeMembership> GetCommitteeMembershipAsync(int committeeId, string memberId);
    }
}
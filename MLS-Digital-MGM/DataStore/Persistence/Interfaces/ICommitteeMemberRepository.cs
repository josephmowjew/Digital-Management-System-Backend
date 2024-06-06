using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface ICommitteeMemberRepository : IRepository<CommitteeMembership>
    {
        // Additional methods specific to the CommitteeMember entity, if needed
    }
}
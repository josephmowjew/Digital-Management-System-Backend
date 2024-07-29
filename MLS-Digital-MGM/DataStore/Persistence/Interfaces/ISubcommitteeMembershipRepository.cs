using DataStore.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface ISubcommitteeMembershipRepository : IRepository<SubcommitteeMembership>
    {
        // Additional methods specific to the SubcommitteeMembership entity, if needed

        Task<SubcommitteeMembership> GetByIdAsync(int subcommitteeMembershipId);

        Task<SubcommitteeMembership> GetSubcommitteeMembershipAsync(int subcommitteeId, string memberShipId);
    }
}

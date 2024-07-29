using DataStore.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface ISubcommitteeRepository : IRepository<Subcommittee>
    {
        // Additional methods specific to the Subcommittee entity, if needed

        Task<int> GetSubcommitteeCount();
        /*Task<IEnumerable<Subcommittee>> GetSubcommitteesByCommitteeId(int committeeId);
        Task<Subcommittee> GetSubcommitteeWithChairperson(int subcommitteeId);
        Task<IEnumerable<Subcommittee>> GetSubcommitteesWithMemberships();*/
    }
}

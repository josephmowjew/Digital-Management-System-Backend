using DataStore.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thread = DataStore.Core.Models.Thread;

namespace DataStore.Persistence.Interfaces
{
    public interface ISubcommitteeThreadRepository : IRepository<SubcommitteeThread>
    {
        // Additional methods specific to the SubcommitteeThread entity, if needed

        Task<IEnumerable<SubcommitteeThread>> GetAllAsync(int subcommitteeId);
    }
}

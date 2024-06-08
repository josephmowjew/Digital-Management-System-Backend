using DataStore.Core.Models;
using System.Collections.Generic;
using Thread = DataStore.Core.Models.Thread;

namespace DataStore.Persistence.Interfaces
{
    public interface IThreadRepository : IRepository<Thread>
    {
        // Additional methods specific to the Thread entity, if needed

         Task<IEnumerable<Thread>> GetAllAsync(int committeeId);
    }

   
}
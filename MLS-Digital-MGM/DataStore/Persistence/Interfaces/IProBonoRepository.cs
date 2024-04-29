using DataStore.Core.Models;

namespace DataStore.Persistence.Interfaces
{
    public interface IProBonoRepository : IRepository<ProBono>
    {
        // Additional methods specific to the ProBono entity, if needed

        Task<ProBono?> GetLastProBonoAsync();
    }
}

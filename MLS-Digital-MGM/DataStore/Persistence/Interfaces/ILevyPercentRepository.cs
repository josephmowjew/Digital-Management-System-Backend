using DataStore.Core.Models;

namespace DataStore.Persistence.Interfaces
{
    public interface ILevyPercentRepository : IRepository<LevyPercent>
    {
        // Additional methods specific to the LevyPercent entity, if needed

        Task<LevyPercent> GetCurrentLevyPercentageAsync();
    }
}

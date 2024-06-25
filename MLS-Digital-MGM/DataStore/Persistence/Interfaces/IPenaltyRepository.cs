using DataStore.Core.Models;

namespace DataStore.Persistence.Interfaces
{
    public interface IPenaltyRepository : IRepository<Penalty>
    {
        // Additional methods specific to the Penalty entity, if needed

        Task<List<Penalty>> GetByMemberId(int memberId);

        Task<Penalty> DeleteAsync(Penalty penalty);

    }
}


using DataStore.Core.Models;

namespace DataStore.Persistence.Interfaces
{
    public interface INotaryPublicRepository : IRepository<NotaryPublic>
    {
        // Additional methods specific to the NotaryPublic entity, if needed

        Task<List<NotaryPublic>> GetByMemberId(int memberId);

        Task<NotaryPublic> DeleteAsync(NotaryPublic notaryPublic);
        Task<int> GetNotaryPublicCountAsync();
        Task<NotaryPublic> GetNotariesPublicByMemberIdAsync(int memberId);
    }
}

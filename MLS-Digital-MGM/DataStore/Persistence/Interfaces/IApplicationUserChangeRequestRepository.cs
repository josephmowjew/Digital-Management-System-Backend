using DataStore.Core.Models;

namespace DataStore.Persistence.Interfaces
{
    public interface IApplicationUserChangeRequestRepository : IRepository<ApplicationUserChangeRequest>
    {
        Task<ApplicationUserChangeRequest> GetByIdAsync(int id);
        Task<IEnumerable<ApplicationUserChangeRequest>> GetByUserIdAsync(string userId);
        Task<ApplicationUserChangeRequest> GetPendingRequestByUserIdAsync(string userId);
    }
}

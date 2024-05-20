using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface ILicenseApprovalHistoryRepository : IRepository<LicenseApprovalHistory>
    {
        // Additional methods specific to the LicenseApprovalHistory entity, if needed
        Task<List<LicenseApprovalHistory>?> GetLicenseApprovalHistoryByLicenseApplication(int id);
    }
}

using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface ILicenseApplicationRepository : IRepository<LicenseApplication>
    {
        // Additional methods specific to the LicenseApplication entity, if needed
        Task<bool> HasPreviousApplicationsAsync(int memberId);
    }
}

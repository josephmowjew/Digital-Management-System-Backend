using DataStore.Core.Models;
using System;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface ILicenseApprovalLevelRepository : IRepository<LicenseApprovalLevel>
    {
        Task<LicenseApprovalLevel?> GetLicenseApprovalLevelByLevel(int level);

        // Additional methods specific to the LicenseApprovalLevel entity, if needed

        Task<LicenseApprovalLevel> GetNextApprovalLevel(LicenseApprovalLevel licenseApprovalLevel);
    }
}
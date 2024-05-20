using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataStore.Persistence.SQLRepositories
{
    public class LicenseApprovalLevelRepository : Repository<LicenseApprovalLevel>, ILicenseApprovalLevelRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public LicenseApprovalLevelRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<LicenseApprovalLevel?> GetLicenseApprovalLevelByLevel(int level)
        {
           return await this._context.LicenseApprovalLevels.FirstOrDefaultAsync(l => l.Level == level);
        }

        public Task<LicenseApprovalLevel> GetNextApprovalLevel(LicenseApprovalLevel licenseApprovalLevel)
        {
            // get license approval level that is higher than the approval level with the id passed in
            return _context.LicenseApprovalLevels.Include(l => l.Department).OrderBy(x => x.Level).Where(x => x.Level > licenseApprovalLevel.Level).FirstOrDefaultAsync();
        }
    }
}
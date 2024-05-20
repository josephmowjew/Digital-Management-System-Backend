using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class LicenseApplicationRepository : Repository<LicenseApplication>, ILicenseApplicationRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public LicenseApplicationRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<LicenseApplication> GetByIdAsync(int id)
        {
           return await this._context.LicenseApplications
           .Include(l => l.Member)
           .ThenInclude(m => m.User)
           .Include(m => m.Member.Firm)
           .Include(l => l.CurrentApprovalLevel)
           .Include(l => l.CurrentApprovalLevel.Department)
           .Include(l => l.YearOfOperation)
           .Include(l => l.Attachments)
           .ThenInclude(a => a.AttachmentType)
           .Include(l => l.CreatedBy)
           .Where(l => l.Id == id && l.Status != Lambda.Deleted)
           .FirstOrDefaultAsync();
        }

        public async Task<bool> HasPreviousApplicationsAsync(int memberId)
        {
            return await this._context.LicenseApplications
                .Where(l => l.MemberId == memberId &&
                            l.ApplicationStatus != Lambda.Pending &&
                            l.ApplicationStatus != Lambda.Draft &&
                            l.ApplicationStatus != Lambda.Denied)
                .AnyAsync();
        }
    }
}


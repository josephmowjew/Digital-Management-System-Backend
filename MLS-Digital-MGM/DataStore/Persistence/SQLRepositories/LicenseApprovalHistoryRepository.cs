using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class LicenseApprovalHistoryRepository : Repository<LicenseApprovalHistory>, ILicenseApprovalHistoryRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public LicenseApprovalHistoryRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<LicenseApprovalHistory>?> GetLicenseApprovalHistoryByLicenseApplication(int applicationId)
        {
           return await this._context.LicenseApprovalHistories.Include(lh => lh.ChangedBy).Where(l => l.LicenseApplicationId == applicationId).ToListAsync();
        }
    }
}

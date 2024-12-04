using DataStore.Persistence.Interfaces;
using DataStore.Core.Models;
using DataStore.Helpers;
using Microsoft.EntityFrameworkCore;
using DataStore.Data;

namespace DataStore.Persistence.SQLRepositories
{
    public class ApplicationUserChangeRequestRepository : Repository<ApplicationUserChangeRequest>, IApplicationUserChangeRequestRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationUserChangeRequestRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplicationUserChangeRequest> GetByIdAsync(int id)
        {
            return await _context.ApplicationUserChangeRequests
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedDate == null);
        }

        public async Task<IEnumerable<ApplicationUserChangeRequest>> GetAllAsync()
        {
            return await _context.ApplicationUserChangeRequests
                .Include(x => x.User)
                .Where(x => x.DeletedDate == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUserChangeRequest>> GetByUserIdAsync(string userId)
        {
            return await _context.ApplicationUserChangeRequests
                .Include(x => x.User)
                .Where(x => x.UserId == userId && x.DeletedDate == null)
                .ToListAsync();
        }


        public async Task<ApplicationUserChangeRequest> GetPendingRequestByUserIdAsync(string userId)
        {
            return await _context.ApplicationUserChangeRequests
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.Status == Lambda.Pending &&
                    x.DeletedDate == null);
        }
    }
}


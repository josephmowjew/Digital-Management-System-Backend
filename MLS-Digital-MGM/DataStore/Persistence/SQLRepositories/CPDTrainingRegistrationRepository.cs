using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class CPDTrainingRegistrationRepository : Repository<CPDTrainingRegistration>, ICPDTrainingRegistrationRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CPDTrainingRegistrationRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<CPDTrainingRegistration>> GetAll(Expression<Func<CPDTrainingRegistration, bool>> value)
        {
            return await _context.CPDTrainingRegistrations.Include(t => t.CPDTraining).Include(t => t.Member).ThenInclude(m => m.User).Include(t => t.Attachments).Include(r => r.CreatedBy).Where(value).ToListAsync();
        }

        public async Task<CPDTrainingRegistration> GetByIdAsync(int id)
        {
            return await _context.CPDTrainingRegistrations.Include(t => t.CPDTraining).Include(t => t.Member).Include(t => t.Attachments).Include(r => r.CreatedBy).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<int> GetCpdTrainingsAttendedCountByUserAsync(int memberId)
        {
            return await _context.CPDTrainingRegistrations
                .Where(p => p.MemberId == memberId && p.Status != Lambda.Deleted && p.RegistrationStatus == Lambda.Attended).CountAsync(); ;
        }
    }
}

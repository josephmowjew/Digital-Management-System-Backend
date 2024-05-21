using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        // public async Task<CPDTrainingRegistration> GetByIdAsync(int id)
        // {
        //     return await _context.CPDTrainingRegistrations.Include(r => r.CreatedBy).FirstOrDefaultAsync(r => r.Id == id);
        // }
    }
}

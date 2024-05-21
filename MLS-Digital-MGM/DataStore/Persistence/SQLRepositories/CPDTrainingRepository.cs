using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class CPDTrainingRepository : Repository<CPDTraining>, ICPDTrainingRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CPDTrainingRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<CPDTraining> GetByIdAsync(int id)
        {
            return await _context.CPDTrainings
            .Include(t => t.CreatedBy)
            .Include(t => t.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .Include(t => t.YearOfOperation)
            .Include(t => t.CPDUnitsEarned)
            .Include(t => t.CPDTrainingRegistration)
            .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}

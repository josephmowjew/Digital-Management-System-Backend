using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class PenaltyRepository : Repository<Penalty>, IPenaltyRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public PenaltyRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<Penalty> GetByIdAsync(int id)
        {
            return await _context.Penalties
            .Include(t => t.CreatedBy)
            .Include(t => t.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .Include(t => t.YearOfOperation)
            .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Additional methods specific to the Penalty entity, if needed
    }
}

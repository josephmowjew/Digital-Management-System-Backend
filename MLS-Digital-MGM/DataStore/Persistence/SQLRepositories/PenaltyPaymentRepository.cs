using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class PenaltyPaymentRepository : Repository<PenaltyPayment>, IPenaltyPaymentRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public PenaltyPaymentRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<PenaltyPayment> GetByIdAsync(int id)
        {
            return await _context.PenaltyPayments
            .Include(t => t.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .FirstOrDefaultAsync(t => t.Id == id);
        }
        // Additional methods specific to the PenaltyPayment entity, if needed
    }
}

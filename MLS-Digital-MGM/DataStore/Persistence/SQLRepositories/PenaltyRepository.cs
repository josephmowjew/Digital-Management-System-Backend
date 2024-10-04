using DataStore.Core.Models;
using DataStore.Core.Models.Interfaces;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
            .Include(t => t.PenaltyPayments)
            .Include(t => t.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .Include(t => t.YearOfOperation)
            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Penalty>> GetByMemberId(int memberId) { 
            return await _context.Penalties.Where(penalty => penalty.MemberId ==  memberId && penalty.Status != Lambda.Deleted).ToListAsync();
        }

        public async Task<Penalty> DeleteAsync(Penalty penalty)
        {
            if (penalty != null)
            {
                penalty.DeletedDate = DateTime.Now;
                penalty.Status = Lambda.Deleted;

                // Mark related Penalty payments as deleted
                var payments = _context.PenaltyPayments
                                           .Where(app => app.PenaltyId == penalty.Id && app.Status != "Deleted")
                                           .ToList();

                //Console.WriteLine("Hello");
                if (payments.Any())
                {
                    foreach (var payment in payments)
                    {
                        payment.DeletedDate = DateTime.Now;
                        payment.Status = Lambda.Deleted;
                    }
                }
            }

            return penalty;

        }

        public async Task<int> GetPenaltyCountAsync(){
            return await _context.Penalties
                .Where(pb => pb.Status == Lambda.Active)
                .CountAsync();
        }
    }
}

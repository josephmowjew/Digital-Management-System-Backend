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
            }

            //get all penalty payments associated with the penalty

            var penaltyPayments = penalty.PenaltyPayments.ToList();

            foreach (var item in penaltyPayments)
            {
                item.DeletedDate = DateTime.Now;
                item.Status = Lambda.Deleted;
               
            }

            return penalty;

        }

        // Additional methods specific to the Penalty entity, if needed
    }
}

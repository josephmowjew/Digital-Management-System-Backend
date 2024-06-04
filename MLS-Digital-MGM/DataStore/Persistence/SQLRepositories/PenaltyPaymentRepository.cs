using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
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

        // Additional methods specific to the PenaltyPayment entity, if needed
    }
}

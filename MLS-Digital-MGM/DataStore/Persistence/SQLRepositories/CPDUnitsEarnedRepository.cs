using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class CPDUnitsEarnedRepository : Repository<CPDUnitsEarned>, ICPDUnitsEarnedRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CPDUnitsEarnedRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public Task<int> GetSummedCPDUnitsEarnedByMemberId(int memberId)
        {
            return _context.CPDUnitsEarned.Include(c => c.Member).Include(c => c.CPDTraining).Include(c => c.YearOfOperation)
               .Where(c => c.MemberId == memberId).SumAsync(c => c.UnitsEarned);
        }

    }
}

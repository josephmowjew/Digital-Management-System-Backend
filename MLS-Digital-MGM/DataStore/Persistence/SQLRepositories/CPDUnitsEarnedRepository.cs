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

        // public async Task<CPDUnitsEarned> GetByIdAsync(int id)
        // {
        //     return await _context.CPDUnitsEarned.Include(c => c.CreatedBy).FirstOrDefaultAsync(c => c.Id == id);
        // }
    }
}

using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class LevyPercentRepository : Repository<LevyPercent>, ILevyPercentRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public LevyPercentRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<LevyPercent> GetByIdAsync(int id)
        {
            return await _context.LevyPercents
            .Include(t => t.YearOfOperation)
            .Where(t => t.Id == id && t.Status != Lambda.Deleted)
            .FirstOrDefaultAsync();
        }

        public async Task<List<LevyPercent>> GetAllAsync()
        {
            return await _context.LevyPercents.ToListAsync();
        }

        public async Task<LevyPercent> DeleteAsync(LevyPercent levyPercent)
        {
            if (levyPercent != null)
            {
                levyPercent.Status = Lambda.Deleted;
            }

            return levyPercent;
        }

        public async Task<LevyPercent> GetCurrentLevyPercentageAsync()
        {
            // get current year of operation based on the current date
            return await _context.LevyPercents.FirstOrDefaultAsync(l => l.OperationStatus == Lambda.Current && l.Status != Lambda.Deleted);
        }
    }
}

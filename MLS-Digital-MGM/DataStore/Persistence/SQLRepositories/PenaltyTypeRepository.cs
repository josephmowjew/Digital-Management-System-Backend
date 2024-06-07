using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class PenaltyTypeRepository : Repository<PenaltyType>, IPenaltyTypeRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public PenaltyTypeRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<PenaltyType?> GetSingleAsync(Expression<Func<PenaltyType, bool>> predicate)
        {
            return await _context.PenaltyTypes.FirstOrDefaultAsync(predicate);
        }
    }
}

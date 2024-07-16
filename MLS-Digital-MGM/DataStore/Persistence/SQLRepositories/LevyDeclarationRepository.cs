using DataStore.Core.Models;
using DataStore.Core.Models.Interfaces;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class LevyDeclarationRepository : Repository<LevyDeclaration>, ILevyDeclarationRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public LevyDeclarationRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<LevyDeclaration> GetByIdAsync(int id)
        {
            return await _context.LevyDeclarations.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<LevyDeclaration>> GetAllAsync()
        {
            return await _context.LevyDeclarations.ToListAsync();
        }

        public async Task<LevyDeclaration> DeleteAsync(LevyDeclaration levyDeclaration)
        {
            if (levyDeclaration != null)
            {
                levyDeclaration.Status = Lambda.Deleted;
            }

            return levyDeclaration;
        }
    }
}

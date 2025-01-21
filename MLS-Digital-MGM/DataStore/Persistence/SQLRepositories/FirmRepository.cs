using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class FirmRepository : Repository<Firm>, IFirmRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public FirmRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<Firm> GetByIdAsync(int id)
        {
            return await _context.Firms
                .Include(f => f.CreatedBy)
                .Include(f => f.Customer)
                .Include(f => f.InstitutionType)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<int> GetFirmsCountAsync(){
            return await _context.Firms
                .Where(fm => fm.Status == Lambda.Active)
                .CountAsync();
        }
    }
}

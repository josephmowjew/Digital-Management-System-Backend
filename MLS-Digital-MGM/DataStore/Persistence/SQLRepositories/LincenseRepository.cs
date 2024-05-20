using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class LicenseRepository : Repository<License>, ILicenseRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public LicenseRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

       public async Task<License?> GetLastLicenseNumber(int YearOfOperationId)
        {
            return await _context.Licenses
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(x => x.YearOfOperationId == YearOfOperationId);
        }
    }
}

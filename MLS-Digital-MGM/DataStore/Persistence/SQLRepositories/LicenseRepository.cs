using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task<License?> GetLicenseById(int id){
            return await this._context.Licenses
           .Include(l => l.Member)
           .ThenInclude(m => m.User)
           .Include(m => m.Member.Firm)
           .Where(l => l.Id == id && l.Status != Lambda.Deleted)
           .FirstOrDefaultAsync();
        }

       public async Task<License?> GetLastLicenseNumber(int YearOfOperationId)
        {
            return await _context.Licenses
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(x => x.YearOfOperationId == YearOfOperationId);
        }

       public async Task<License?> GetSingleAsync(Expression<Func<License, bool>> predicate)
        {
            return await _context.Licenses.FirstOrDefaultAsync(predicate);
        }

       public async Task<License?> GetLicenseByLicenseNumber(string licenseNumber)
        {
            return await _context.Licenses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.LicenseNumber == licenseNumber);
        }

       public async Task<int> CountAsync()
        {
            return await _context.Licenses
                .CountAsync(l => l.Status == Lambda.Active);
        }

    public async Task<License?> GetLicenseByMemberId(int memberId)
    {
        return await _context.Licenses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MemberId == memberId);
    }

    }
}

using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using DataStore.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Persistence.SQLRepositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public CountryRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        // Implement additional methods here for country-specific operations
        public async Task<int> GetCountriesCountAsync()
        {
            return await _context.Countries
                .Where(c => c.Status == Lambda.Active)
                .CountAsync();
        }

    }
}
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
    public class YearOfOperationRepository : Repository<YearOfOperation>, IYearOfOperationRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public YearOfOperationRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<YearOfOperation> GetCurrentYearOfOperation()
        {
            // get current year of operation based on the current date
            return await _context.YearOfOperations.FirstOrDefaultAsync(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now);
        }
    }
}

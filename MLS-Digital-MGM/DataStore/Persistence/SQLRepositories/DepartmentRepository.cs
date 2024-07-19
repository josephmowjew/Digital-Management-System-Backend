using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Persistence.SQLRepositories
{
    public class DepartmentRepository: Repository<Department>, IDepartmentRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<int> GetDepartmentsCountAsync(){
            return await _context.Departments
                .Where(dp => dp.Status == Lambda.Active)
                .CountAsync();
        }
         public async Task<List<Department>> GetDepartmentsByIdsAsync(IEnumerable<int> departmentIds)
        {
            return await _context.Departments
                .Where(d => departmentIds.Contains(d.Id))
                .ToListAsync();
        }
    }
}

using DataStore.Core.Models.Interfaces;
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
    public class Repository<T>: IRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public Repository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            var query = _context.Set<T>().Where(predicate);
            var result = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return result;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await Task.CompletedTask; // No need for asynchronous update
        }

        public async Task DeleteAsync(int id)
        {
            var entityToDelete = await GetByIdAsync(id);
            if (entityToDelete != null)
            {
                entityToDelete.DeletedDate = DateTime.Now;
                entityToDelete.Status = Lambda.Deleted;
            }
        }

       
    }
}

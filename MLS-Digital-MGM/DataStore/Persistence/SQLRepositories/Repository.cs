using DataStore.Core.Models.Interfaces;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
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
            var type = typeof(T);
            var property = type.GetProperty("Id");
            
            if (property == null)
            {
                throw new ArgumentException($"The entity of type {typeof(T).Name} does not have an Id property.");
            }

            var parameter = Expression.Parameter(type, "record");
            var propertyAccess = Expression.Property(parameter, property);
            var idValue = Expression.Constant(id);
            var equal = Expression.Equal(propertyAccess, idValue);
            var statusCondition = Expression.NotEqual(Expression.Property(parameter, "Status"), Expression.Constant(Lambda.Deleted));
            var andCondition = Expression.AndAlso(statusCondition, equal);
            var lambda = Expression.Lambda<Func<T, bool>>(andCondition, parameter);

            return await _context.Set<T>().FirstOrDefaultAsync(lambda);
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().Where(q => q.Status != Lambda.Deleted).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                                 .Where(q => q.Status != Lambda.Deleted && predicate.Compile()(q))
                                 .ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(q => q.Status != Lambda.Deleted).Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(PagingParameters<T> pagingParameters)
        {
            var query = _context.Set<T>().Where(pagingParameters.Predicate);

            if (!string.IsNullOrEmpty(pagingParameters.SearchTerm))
            {
                // Use reflection to get the properties of the entity type
                var entityProperties = typeof(T).GetProperties();
                foreach (var property in entityProperties)
                {
                    var parameter = Expression.Parameter(typeof(T), "record");
                    var propertyAccess = Expression.Property(parameter, property);
                    var searchTermValue = Expression.Constant(pagingParameters.SearchTerm, typeof(string));
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var containsExpression = Expression.Call(propertyAccess, containsMethod, searchTermValue);
                    var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
                    query = query.Where(lambda);
                }
            }

            // Include the specified navigation properties
            foreach (var include in pagingParameters.Includes)
            {
                query = query.Include(include);
            }

            //query = query.Where(q => q.Status != Lambda.Deleted);

            // Apply sort ordering
            if (!string.IsNullOrEmpty(pagingParameters.SortColumn) && !string.IsNullOrEmpty(pagingParameters.SortDirection))
            {
                query = query.OrderBy($"{pagingParameters.SortColumn} {pagingParameters.SortDirection.ToLower()}");
            }
            var result = await query.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize).Take(pagingParameters.PageSize).ToListAsync();

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

        public async Task DeleteAsync(T entity)
        {
           
            if (entity != null)
            {
                entity.DeletedDate = DateTime.Now;
                entity.Status = Lambda.Deleted;
            }
        }


    }
}

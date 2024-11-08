using DataStore.Core.Models.Interfaces;
using DataStore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DataStore.Persistence.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetPagedAsync(PagingParameters<T> pagingParameters);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task AddRange (List<T> entities);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate,Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,string orderByDirection,int count,int skip,Expression<Func<T, object>>[] includes = null);
        Task<int> CountAsync(PagingParameters<T> pagingParameters);
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate);

        // New method to project entities to DTOs
        //Task<IEnumerable<TDto>> ProjectToAsync<TDto>(Expression<Func<T, bool>> predicate);
    }
}

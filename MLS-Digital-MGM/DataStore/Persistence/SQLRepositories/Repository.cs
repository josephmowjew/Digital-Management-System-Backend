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
using System.Reflection;

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
                                 .Where(predicate)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            string orderByDirection,
            int count,
            int skip,
            Expression<Func<T, object>>[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            query = query.Where(predicate);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    var includeString = GetIncludePath(include);
                    query = query.Include(includeString);

                    // Additional includes for specific navigation properties
                    if (includeString == "Attachments")
                    {
                        query = query.Include("Attachments.AttachmentType");
                    }
                    if (includeString == "CurrentApprovalLevel")
                    {
                        query = query.Include("CurrentApprovalLevel.Department");
                    }
                    if (includeString == "Member")
                    {
                        query = query.Include("Member.User");
                    }
                }
            }

            if (orderBy != null)
            {
                if (orderByDirection.ToUpper() == "ASC")
                {
                    query = orderBy(query);
                }
                else if (orderByDirection.ToUpper() == "DESC")
                {
                    query = orderBy(query).Reverse();
                }
                else
                {
                    throw new ArgumentException("Invalid sort direction. Please use ASC or DESC.");
                }
            }

            if (count > 0)
            {
                query = query.Skip(skip).Take(count);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(q => q.Status != Lambda.Deleted).Where(predicate).FirstOrDefaultAsync();
        }


        public async Task<int> CountAsync(PagingParameters<T> pagingParameters)
        {
            var query = _context.Set<T>().Where(pagingParameters.Predicate);

            if (!string.IsNullOrEmpty(pagingParameters.SearchTerm))
            {
                var entityProperties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var parameter = Expression.Parameter(typeof(T), "record");
                var searchTermValue = Expression.Constant(pagingParameters.SearchTerm.ToLower(), typeof(string));

                var searchExpressions = entityProperties
                    .Where(p => p.PropertyType == typeof(string))
                    .Select(p =>
                    {
                        var propertyAccess = Expression.Property(parameter, p);
                        var toLowerExpression = Expression.Call(propertyAccess, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var containsExpression = Expression.Call(toLowerExpression, containsMethod, searchTermValue);
                        return containsExpression;
                    });

                var orExpression = searchExpressions.Aggregate<Expression>(Expression.OrElse);
                var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);

                query = query.Where(lambda);
            }

            foreach (var include in pagingParameters.Includes)
            {
                var includeString = GetIncludePath(include);
                query = query.Include(includeString);

                if (includeString == "Attachments")
                {
                    query = query.Include("Attachments.AttachmentType");
                }
                if (includeString == "CurrentApprovalLevel")
                {
                    query = query.Include("CurrentApprovalLevel.Department");
                }
                if (includeString == "Member")
                {
                    query = query.Include("Member.User");
                }
            }

            if (!string.IsNullOrEmpty(pagingParameters.SortColumn) && !string.IsNullOrEmpty(pagingParameters.SortDirection))
            {
                query = query.OrderBy($"{pagingParameters.SortColumn} {pagingParameters.SortDirection.ToLower()}");
            }

            if (pagingParameters.CreatedById != null)
            {
                if (typeof(T).GetProperty("CreatedById") != null)
                {
                    query = query.Where(x => EF.Property<string>(x, "CreatedById") == pagingParameters.CreatedById);
                }
            }

            var result = await query.CountAsync();  

            return result;
        }

        public async Task<IEnumerable<T>> GetPagedAsync(PagingParameters<T> pagingParameters)
        {
            var query = _context.Set<T>().Where(pagingParameters.Predicate);

            if (!string.IsNullOrEmpty(pagingParameters.SearchTerm))
            {
                var entityProperties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var parameter = Expression.Parameter(typeof(T), "record");
                var searchTermValue = Expression.Constant(pagingParameters.SearchTerm.ToLower(), typeof(string));

                var searchExpressions = entityProperties
                    .Where(p => p.PropertyType == typeof(string))
                    .Select(p =>
                    {
                        var propertyAccess = Expression.Property(parameter, p);
                        var toLowerExpression = Expression.Call(propertyAccess, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
                        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        var containsExpression = Expression.Call(toLowerExpression, containsMethod, searchTermValue);
                        return containsExpression;
                    });

                var orExpression = searchExpressions.Aggregate<Expression>(Expression.OrElse);
                var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);

                query = query.Where(lambda);
            }

            foreach (var include in pagingParameters.Includes)
            {
                var includeString = GetIncludePath(include);
                query = query.Include(includeString);

                if (includeString == "Attachments")
                {
                    query = query.Include("Attachments.AttachmentType");
                }
                if (includeString == "CurrentApprovalLevel")
                {
                    query = query.Include("CurrentApprovalLevel.Department");
                }
                if (includeString == "Member")
                {
                    query = query.Include("Member.User");
                }
            }

            if (!string.IsNullOrEmpty(pagingParameters.SortColumn) && !string.IsNullOrEmpty(pagingParameters.SortDirection))
            {
                query = query.OrderBy($"{pagingParameters.SortColumn} {pagingParameters.SortDirection.ToLower()}");
            }

            if (pagingParameters.CreatedById != null)
            {
                if (typeof(T).GetProperty("CreatedById") != null)
                {
                    query = query.Where(x => EF.Property<string>(x, "CreatedById") == pagingParameters.CreatedById);
                }
            }

            var result = await query.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize).Take(pagingParameters.PageSize).ToListAsync();

            return result;
        }

        private string GetIncludePath(Expression<Func<T, object>> includeExpression)
        {
            if (includeExpression.Body is MemberExpression memberExpression)
            {
                return GetFullPropertyPath(memberExpression);
            }
            else if (includeExpression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression innerMemberExpression)
            {
                return GetFullPropertyPath(innerMemberExpression);
            }
            else
            {
                throw new InvalidOperationException("Invalid include expression");
            }
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(q => q.Status != Lambda.Deleted).Where(predicate).FirstOrDefaultAsync();
        }

        private string GetFullPropertyPath(MemberExpression memberExpression)
        {
            var path = new List<string>();
            while (memberExpression != null)
            {
                path.Insert(0, memberExpression.Member.Name);
                memberExpression = memberExpression.Expression as MemberExpression;
            }
            return string.Join(".", path);
        }

        public async Task AddRange(List<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
           
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

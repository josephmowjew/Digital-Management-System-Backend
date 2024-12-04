using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace DataStore.Persistence.SQLRepositories
{
  public class MemberRepository : Repository<Member>, IMemberRepository
  {
    protected readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public MemberRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork) 
    {
      this._context = context;
      this._unitOfWork = unitOfWork;
    }

        public async Task<Member?> GetMemberByUserId(string userId)
        {
            // First get the member with basic user info
            var member = await _context.Members
                .Include(m => m.User)
                    .ThenInclude(u => u.ProfilePictures.Where(p => p.Status != Lambda.Deleted))
                    .ThenInclude(p => p.AttachmentType)
                .Include(m => m.Customer)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            // If member exists, fetch and append profile pictures
            if (member?.User != null)
            {
                var picturesCount = await _context.Attachments
                    .CountAsync(a => a.ApplicationUsers.Any(u => u.Id == member.User.Id));

                var pictures = await _context.Attachments
                    .Where(a => a.ApplicationUsers.Any(u => u.Id == member.User.Id) && a.Status != Lambda.Deleted)
                    .ToListAsync();

                member.User.ProfilePictures = pictures;

                // Log for debugging
                System.Diagnostics.Debug.WriteLine($"Found {picturesCount} pictures for user {member.User.Email}");
            }

            return member;
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Customer).Where(q => q.Status != Lambda.Deleted).ToListAsync();
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _context.Members.Include(m => m.User).Include(m => m.Customer).FirstOrDefaultAsync(q => q.Id == id && q.Status != Lambda.Deleted);
        }

        public async Task<int> GetMembersCountAsync(){
            return await _context.Members.CountAsync(q => q.Status == Lambda.Active);
        }

        
        public async Task<int> GetLicensedMembersCountAsync()
        {
            return await _context.Members
                .CountAsync(m => _context.Licenses.Any(l => l.MemberId == m.Id) && m.Status != Lambda.Deleted);
        }

        //get unlicensed members count
        public async Task<int> GetUnlicensedMembersCountAsync()
        {
            return await _context.Members
                .CountAsync(m => !_context.Licenses.Any(l => l.MemberId == m.Id) && m.Status != Lambda.Deleted);
        }

        public async Task<IEnumerable<Member>> GetUnlicensedMembersAsync()
        {
            return await _context.Members
                .Include(m => m.User)
                .Include(m => m.Customer)
                .Where(m => !_context.Licenses.Any(l => l.MemberId == m.Id) && m.Status != Lambda.Deleted)
                .ToListAsync();
        }

        
        public async Task<IEnumerable<Member>> GetLicensedMembersAsync()
        {
            return await _context.Members
                .Include(m => m.User)
                .Include(m => m.Customer)
                .Include(m => m.Firm)
                .Where(m => _context.Licenses.Any(l => l.MemberId == m.Id) && m.Status != Lambda.Deleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Member>> GetMembersByFirmIdAsync(int firmId)
        {
            return await _context.Members
                .Include(m => m.User)
                .Include(m => m.Customer)
                .Include(m => m.Firm)
                .Where(m => m.FirmId == firmId && m.Status != Lambda.Deleted)
                .ToListAsync();
        }

        public new async Task<IEnumerable<Member>> GetPagedAsync(PagingParameters<Member> pagingParameters)
        {
            var query = _context.Set<Member>().Where(pagingParameters.Predicate);

            if (!string.IsNullOrEmpty(pagingParameters.SearchTerm))
            {
                var entityProperties = typeof(Member).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var parameter = Expression.Parameter(typeof(Member), "record");
                var searchTermValue = Expression.Constant(pagingParameters.SearchTerm.ToLower(), typeof(string));

                var searchExpressions = entityProperties
                    .SelectMany(p =>
                    {
                        // Check if property is a string and add it directly
                        if (p.PropertyType == typeof(string))
                        {
                            var propertyAccess = Expression.Property(parameter, p);
                            var toLowerExpression = Expression.Call(propertyAccess, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            var containsExpression = Expression.Call(toLowerExpression, containsMethod, searchTermValue);
                            return new[] { containsExpression };
                        }

                        // If the property is a class or association, get its properties
                        if (p.PropertyType.IsClass && p.PropertyType != typeof(string))
                        {
                            var nestedProperties = p.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Where(nestedProp => nestedProp.PropertyType == typeof(string));

                            return nestedProperties.Select(nestedProp =>
                            {
                                var nestedAccess = Expression.Property(Expression.Property(parameter, p), nestedProp);
                                var nestedToLowerExpression = Expression.Call(nestedAccess, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
                                var nestedContainsExpression = Expression.Call(nestedToLowerExpression, typeof(string).GetMethod("Contains", new[] { typeof(string) }), searchTermValue);
                                return (Expression)nestedContainsExpression;
                            });
                        }

                        return Enumerable.Empty<Expression>();
                    })
                    .ToList();

                if (searchExpressions.Any())
                {
                    var orExpression = searchExpressions.Aggregate<Expression>(Expression.OrElse);
                    var lambda = Expression.Lambda<Func<Member, bool>>(orExpression, parameter);
                    query = query.Where(lambda);
                }
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
                 if (includeString == "Members")
                {
                    query = query.Include("Members.User");
                }
            }

            //filter licenses to only include active licenses
           
            query = query.Include(m => m.Licenses.Where(l => l.Status == Lambda.Active));
            

            if (!string.IsNullOrEmpty(pagingParameters.SortColumn) && !string.IsNullOrEmpty(pagingParameters.SortDirection))
            {
                query = query.OrderBy($"{pagingParameters.SortColumn} {pagingParameters.SortDirection.ToLower()}");
            }

            if (pagingParameters.CreatedById != null)
            {
                if (typeof(Member).GetProperty("CreatedById") != null)
                {
                    query = query.Where(x => EF.Property<string>(x, "CreatedById") == pagingParameters.CreatedById);
                }
            }

            var result = await query.Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize).Take(pagingParameters.PageSize).ToListAsync();

            return result;
        }

         private string GetIncludePath(Expression<Func<Member, object>> includeExpression)
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


        
    }
}

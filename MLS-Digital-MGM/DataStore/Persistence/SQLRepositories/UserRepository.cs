using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace DataStore.Persistence.SQLRepositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public UserRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public UserRepository(ApplicationDbContext context, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager) :base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
       
        public  async Task<IdentityResult> AddAsync(ApplicationUser entity, string password)
        {
            return await this._userManager.CreateAsync(entity, password);
        }

        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser applicationUser, string roleName)
        {
            //remove user from all roles
            await this._userManager.RemoveFromRolesAsync(applicationUser, await this._userManager.GetRolesAsync(applicationUser));

            //add user to role
            return await this._userManager.AddToRoleAsync(applicationUser, roleName);
        }
        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
           return await _context.Users.Where(x => x.Email == email && x.Status != Lambda.Deleted).FirstOrDefaultAsync();
        }

         public async Task<ApplicationUser?> GetSingleUserNoFilter(string id)
        {

            return await this._context.Users.FirstOrDefaultAsync(u => u.Id == id);

        }

        public async Task<ApplicationUser?> GetSingleUser(string id)
        {

            return await this._context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Status != Lambda.Deleted);

        }

        public async Task<ApplicationUser> ConfirmAccount(string id, int pin)
        {
            var user = await GetSingleUser(id);
            user.EmailConfirmed = true;
            return user;
        }

        public IdentityUserRole<string> GetUserRoleByUserId(string userId)
        {
            return _context.UserRoles.FirstOrDefault(u => u.UserId == userId);
        }

        public void ActivateAccount(ApplicationUser user)
        {
            user.EmailConfirmed = true;
            user.Status = Lambda.Active;
            user.DeletedDate = null;
        }

        public string GetRoleName(string roleId)
        {
            var role = this._context.Roles.FirstOrDefault(r => r.Id == roleId);

            if(role != null)
            {
                return role.Name;
            }else{
                return "";
            }
        }

        public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string finance)
        {
            return (List<ApplicationUser>)await this._userManager.GetUsersInRoleAsync(finance);
        }

        public async Task<int> GetUsersCountAsync(){
            return await _context.Users
                .Where(us => us.EmailConfirmed == true)
                .CountAsync();
        }

        public async Task<List<ApplicationUser>> GetUsersByDepartmentIdsAsync(IEnumerable<int> departmentIds)
        {
            return await _context.Users
                .Where(u => departmentIds.Contains(u.DepartmentId))
                .ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetAllConfirmedUsersAsync()
        {
            return await _context.Users
                .Where(u => u.EmailConfirmed)
                .ToListAsync();
        }

public async Task<IEnumerable<ApplicationUser>> GetPagedStaffUsersAsync(PagingParameters<ApplicationUser> pagingParameters)
{
    // Build the query first
    var query = _context.Users.AsNoTracking() // Add AsNoTracking() for better performance
        .Where(pagingParameters.Predicate)
        .Where(u => !_context.UserRoles.Any(ur => 
            ur.UserId == u.Id && 
            _context.Roles.Any(r => r.Id == ur.RoleId && r.Name.ToLower() == "member")));

    // Apply search if provided
    if (!string.IsNullOrEmpty(pagingParameters.SearchTerm))
    {
        query = query.Where(u => 
            u.UserName.Contains(pagingParameters.SearchTerm) || 
            u.Email.Contains(pagingParameters.SearchTerm));
    }

    // Apply sorting
    if (!string.IsNullOrEmpty(pagingParameters.SortColumn))
    {
        var parameter = Expression.Parameter(typeof(ApplicationUser), "u");
        var property = Expression.Property(parameter, pagingParameters.SortColumn);
        var lambda = Expression.Lambda<Func<ApplicationUser, object>>(
            Expression.Convert(property, typeof(object)), 
            parameter);

        query = pagingParameters.SortDirection == "asc"
            ? query.OrderBy(lambda)
            : query.OrderByDescending(lambda);
    }
    else
    {
        query = query.OrderBy(u => u.UserName);
    }

    // Execute the query in a single operation
    return await query
        .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
        .Take(pagingParameters.PageSize)
        .ToListAsync()
        .ConfigureAwait(false);
}


        public async Task<int> CountStaffUsersAsync(PagingParameters<ApplicationUser> pagingParameters)
        {
            var query = _context.Users
            .Where(pagingParameters.Predicate)
            .Where(u => !_context.UserRoles.Any(ur => ur.UserId == u.Id && _context.Roles.Any(r => r.Id == ur.RoleId && r.Name.ToLower() == "member")));

            // Apply search if provided
            if (!string.IsNullOrEmpty(pagingParameters.SearchTerm))
            {
                var searchTerm = pagingParameters.SearchTerm.ToLower();
                query = query.Where(u => 
                u.UserName.ToLower().Contains(searchTerm) || 
                u.Email.ToLower().Contains(searchTerm) ||
                u.FirstName.ToLower().Contains(searchTerm) ||
                    u.LastName.ToLower().Contains(searchTerm)
                );
            }

            // Apply additional filters if needed
            if (pagingParameters.CreatedById != null)
            {
                query = query.Where(x => x.CreatedById == pagingParameters.CreatedById);
            }

            // Count the results
            var result = await query.CountAsync();

            return result;
        }
    }

}

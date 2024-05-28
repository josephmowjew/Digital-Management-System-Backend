using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<ApplicationUser> FindByEmailAsync(string email)
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
    }
}

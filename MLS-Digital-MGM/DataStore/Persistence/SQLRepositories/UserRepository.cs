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
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public UserRepository(ApplicationDbContext context, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) :base(context, unitOfWork)
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
            //add user to role
            return await this._userManager.AddToRoleAsync(applicationUser, roleName);
        }
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
           return await _context.Users.Where(x => x.Email == email && x.Status != Lambda.Deleted).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser?> GetSingleUser(string id, bool includeRelated = true)
        {

            return await this._context.Users.FirstOrDefaultAsync(u => u.Id == id && u.DeletedDate == null);

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



       

    }
}

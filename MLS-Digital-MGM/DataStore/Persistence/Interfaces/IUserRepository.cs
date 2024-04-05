using DataStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        // Additional methods specific to the User entity, if needed

        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> ConfirmAccount(string id, int pin);
        Task<ApplicationUser?> GetSingleUser(string id, bool includeRelated = true);
    
        IdentityUserRole<string> GetUserRoleByUserId(string userId);
        Task<IdentityResult> AddAsync(ApplicationUser entity, string password);
        Task<IdentityResult> AddUserToRoleAsync(ApplicationUser applicationUser, string roleName);

    }
}

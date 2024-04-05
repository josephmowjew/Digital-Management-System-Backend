using DataStore.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IRoleRepository: IRepository<Role>
    {
        void AddRole(string roleName);
        Task UpdateRoleAsync(Role role);
        Task<List<Role>> GetRolesAsync();
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<Role> GetRoleByIdAsync(string roleId);
        Task DeleteRoleAsync(string roleId);



    }
}

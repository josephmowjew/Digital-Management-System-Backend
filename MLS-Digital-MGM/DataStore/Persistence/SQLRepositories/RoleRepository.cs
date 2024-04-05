using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class RoleRepository: Repository<Role>, IRoleRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public RoleRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;

        }

        //method to get roles async
        public async Task<List<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }


        public void AddRole(string roleName)
        {
            //add role
            _context.Roles.Add(new Role { Name = roleName });
        }

        //method to update role

        public async Task UpdateRoleAsync(Role role)
        {
            var roleToUpdate = await _context.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);
            roleToUpdate.Name = role.Name;

        }


        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        //method to get role by id
        public async Task<Role> GetRoleByIdAsync(string roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        }

        //method to perfom soft delete
        public async Task DeleteRoleAsync(string roleId)
        {
            var roleToDelete = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            roleToDelete.DeletedDate = DateTime.Now;
            roleToDelete.Status = Lambda.Deleted;
        }
       

    }
}

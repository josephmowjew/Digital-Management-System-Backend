using AutoMapper;
using DataStore.Core.DTOs.Role;
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Mappers
{
    public class RoleProfile: Profile
    {
        public RoleProfile()
        {
            //add mappings for role and roleDTOs
            CreateMap<CreateRoleDTO, Role>();
            CreateMap<Role, ReadRoleDTO>();
            CreateMap<UpdateRoleDTO, Role>();
        }
        
    }
}

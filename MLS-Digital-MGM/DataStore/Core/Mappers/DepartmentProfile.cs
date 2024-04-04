using AutoMapper;
using DataStore.Core.DTOs.Department;
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Mappers
{
    public class DepartmentProfile: Profile
    {
        public DepartmentProfile()
        {
            CreateMap<CreateDepartmentDTO, Department>();
            CreateMap<Department, ReadDepartmentDTO>();
            CreateMap<UpdateDepartmentDTO, Department>();
        }
        
    }
}

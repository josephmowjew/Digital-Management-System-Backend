using AutoMapper;
using DataStore.Core.DTOs.InstitutionType;
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Mappers
{
    public class InstitutionTypeProfile: Profile
    {
        public InstitutionTypeProfile()
        {
            CreateMap<CreateInstitutionTypeDTO, InstitutionType>();
            CreateMap<InstitutionType, ReadInstitutionTypeDTO>();
            CreateMap<UpdateInstitutionTypeDTO, InstitutionType>();
        }
        
    }
}

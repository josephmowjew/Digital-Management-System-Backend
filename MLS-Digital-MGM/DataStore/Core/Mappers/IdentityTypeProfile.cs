using AutoMapper;
using DataStore.Core.DTOs.IdentityType;
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Mappers
{
    public class IdentityTypeProfile: Profile
    {
        public IdentityTypeProfile()
        {
            CreateMap<CreateIdentityTypeDTO, IdentityType>();
            CreateMap<IdentityType, ReadIdentityTypeDTO>();
            CreateMap<UpdateIdentityTypeDTO, IdentityType>();
        }
        
    }
}

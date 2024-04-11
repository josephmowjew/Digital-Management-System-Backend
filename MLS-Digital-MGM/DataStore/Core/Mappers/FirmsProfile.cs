//create films profile mapper
using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.Firms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.DTOs;

namespace DataStore.Core.Mappers
{
    public class FirmsProfile : Profile
    {
        public FirmsProfile()
        {
            CreateMap<Firm, ReadFirmDTO>();
            CreateMap<CreateFirmDTO, Firm>();
            CreateMap<UpdateFirmDTO, Firm>();
        }
    }
}
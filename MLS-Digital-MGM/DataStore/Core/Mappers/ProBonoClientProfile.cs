using AutoMapper;
using DataStore.Core.DTOs.ProBonoClient;
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Mappers
{
    public class ProBonoClientProfile : Profile
    {
        public ProBonoClientProfile()
        {
            CreateMap<CreateProBonoClientDTO, ProbonoClient>();
            CreateMap<ProbonoClient, ReadProBonoClientDTO>();
            CreateMap<UpdateProBonoClientDTO, ProbonoClient>();
        }

    }
}

using AutoMapper;
using DataStore.Core.DTOs.ProBonoApplication;
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Mappers
{
    public class ProBonoApplicationProfile: Profile
    {
        public ProBonoApplicationProfile()
        {
            CreateMap<CreateProBonoApplicationDTO, DataStore.Core.Models.ProBonoApplication>()
                .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<UpdateProBonoApplicationDTO, DataStore.Core.Models.ProBonoApplication>()
                .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<DataStore.Core.Models.ProBonoApplication, ReadProBonoApplicationDTO>();
        }
    }
}

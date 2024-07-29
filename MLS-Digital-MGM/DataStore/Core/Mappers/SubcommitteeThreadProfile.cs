using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs;
using DataStore.Core.DTOs.SubcommitteeThread;

namespace DataStore.Core.Mappers
{
    public class SubcommitteeThreadProfile : Profile
    {
        public SubcommitteeThreadProfile()
        {
            CreateMap<SubcommitteeThread, ReadSubcommitteeThreadDTO>();
            CreateMap<CreateSubcommitteeThreadDTO, SubcommitteeThread>();
            CreateMap<UpdateSubcommitteeThreadDTO, SubcommitteeThread>();
        }
    }
}

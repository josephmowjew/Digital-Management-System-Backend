using AutoMapper;
using DataStore.Core.DTOs.Subcommittee;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class SubcommitteeProfile : Profile
    {
        public SubcommitteeProfile()
        {
            CreateMap<CreateSubcommitteeDTO, Subcommittee>();
            CreateMap<Subcommittee, ReadSubcommitteeDTO>();
            CreateMap<UpdateSubcommitteeDTO, Subcommittee>();
        }
    }
}
    
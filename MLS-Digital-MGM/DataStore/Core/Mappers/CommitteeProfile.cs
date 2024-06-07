using AutoMapper;
using DataStore.Core.DTOs.Committee;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class CommitteeProfile : Profile
    {
        public CommitteeProfile()
        {
            CreateMap<CreateCommitteeDTO, Committee>();
            CreateMap<Committee, ReadCommitteeDTO>();
            CreateMap<UpdateCommitteeDTO, Committee>();
        }
    }
}

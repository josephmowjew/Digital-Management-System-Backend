using AutoMapper;
using DataStore.Core.DTOs.SubcommitteeMessage;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class SubcommitteeMessageProfile : Profile
    {
        public SubcommitteeMessageProfile()
        {
            CreateMap<CreateSubcommitteeMessageDTO, SubcommitteeMessage>();
            CreateMap<SubcommitteeMessage, ReadSubcommitteeMessageDTO>();
            CreateMap<UpdateSubcommitteeMessageDTO, SubcommitteeMessage>();
        }
    }
}

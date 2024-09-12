
using AutoMapper;
using DataStore.Core.DTOs.Stamp;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class StampProfile : Profile
    {
        public StampProfile()
        {
            //add mappings for stamp and stampDTOs
            CreateMap<CreateStampDTO, Stamp>()
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<Stamp, ReadStampDTO>();
            CreateMap<UpdateStampDTO, Stamp>()
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());;
        }
    }
}

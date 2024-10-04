
using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.NotaryPublic;

namespace DataStore.Core.Mappers
{
    public class NotaryPublicProfile : Profile
    {
        public NotaryPublicProfile()
        {
            CreateMap<NotaryPublic, ReadNotaryPublicDTO>();
            CreateMap<CreateNotaryPublicDTO, NotaryPublic>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<UpdateNotaryPublicDTO, NotaryPublic>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
        }
    }
}

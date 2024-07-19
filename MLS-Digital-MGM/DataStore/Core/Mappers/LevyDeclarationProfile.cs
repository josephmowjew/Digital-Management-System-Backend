using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.LevyDeclaration;

namespace DataStore.Core.Mappers
{
    public class LevyDeclarationProfile : Profile
    {
        public LevyDeclarationProfile()
        {
            CreateMap<LevyDeclaration, ReadLevyDeclarationDTO>();
            CreateMap<CreateLevyDeclarationDTO, LevyDeclaration>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<UpdateLevyDeclarationDTO, LevyDeclaration>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
        }
    }
}

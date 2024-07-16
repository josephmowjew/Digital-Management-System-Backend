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
            CreateMap<CreateLevyDeclarationDTO, LevyDeclaration>();
            CreateMap<UpdateLevyDeclarationDTO, LevyDeclaration>();
        }
    }
}

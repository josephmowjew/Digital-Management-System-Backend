using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs;
using DataStore.Core.DTOs.CPDQualification;

namespace DataStore.Core.Mappers
{
    public class CPDQualificationProfile : Profile
    {
        public CPDQualificationProfile()
        {
            CreateMap<CPDQualification, ReadCPDQualificationDTO>()
                .ForMember(dest => dest.DateGenerated, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateGenerated)));

            CreateMap<CreateCPDQualificationDTO, CPDQualification>();
        }
    }
}

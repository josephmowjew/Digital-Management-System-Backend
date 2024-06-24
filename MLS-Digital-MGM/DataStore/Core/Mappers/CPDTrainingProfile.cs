using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.CPDTraining;

namespace DataStore.Core.Mappers
{
    public class CPDTrainingProfile : Profile
    {
        public CPDTrainingProfile()
        {
            CreateMap<CPDTraining, ReadCPDTrainingDTO>()
            .ForMember(dest => dest.DateToBeConducted, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateToBeConducted)))
            .ForMember(dest => dest.RegistrationDueDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.RegistrationDueDate)));
            CreateMap<CreateCPDTrainingDTO, CPDTraining>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<UpdateCPDTrainingDTO, CPDTraining>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
        }
    }
}

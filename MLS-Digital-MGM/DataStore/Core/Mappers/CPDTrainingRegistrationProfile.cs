using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.CPDTrainingRegistration;

namespace DataStore.Core.Mappers
{
    public class CPDTrainingRegistrationProfile : Profile
    {
        public CPDTrainingRegistrationProfile()
        {
            CreateMap<CPDTrainingRegistration, ReadCPDTrainingRegistrationDTO>();
            CreateMap<CreateCPDTrainingRegistrationDTO, CPDTrainingRegistration>()
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<UpdateCPDTrainingRegistrationDTO, CPDTrainingRegistration>()
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());;
        }
    }
}

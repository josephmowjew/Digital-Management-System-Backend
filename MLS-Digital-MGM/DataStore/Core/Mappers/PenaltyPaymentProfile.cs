using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.PenaltyPayment;

namespace DataStore.Core.Mappers
{
    public class PenaltyPaymentProfile : Profile
    {
        public PenaltyPaymentProfile()
        {
            CreateMap<PenaltyPayment, ReadPenaltyPaymentDTO>();
            CreateMap<CreatePenaltyPaymentDTO, PenaltyPayment>()
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<UpdatePenaltyPaymentDTO, PenaltyPayment>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
        }
    }
}

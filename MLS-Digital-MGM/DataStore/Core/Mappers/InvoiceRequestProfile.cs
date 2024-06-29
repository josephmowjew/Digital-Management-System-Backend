using AutoMapper;
using DataStore.Core.DTOs.InvoiceRequest;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class InvoiceRequestProfile : Profile
    {
        public InvoiceRequestProfile()
        {
            CreateMap<CreateInvoiceRequestDTO, InvoiceRequest>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.ReferencedEntityType, opt => opt.MapFrom(src => src.ReferencedEntityType))
                .ForMember(dest => dest.ReferencedEntityId, opt => opt.MapFrom(src => src.ReferencedEntityId));
        }
    }
}

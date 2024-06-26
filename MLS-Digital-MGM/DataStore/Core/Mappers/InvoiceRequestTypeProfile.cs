using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.InvoiceRequestType;

namespace DataStore.Core.Mappers
{
    public class InvoiceRequestTypeProfile : Profile
    {
        public InvoiceRequestTypeProfile()
        {
            CreateMap<InvoiceRequestType, ReadInvoiceRequestTypeDTO>();
            CreateMap<CreateInvoiceRequestTypeDTO, InvoiceRequestType>();
            CreateMap<UpdateInvoiceRequestTypeDTO, InvoiceRequestType>();
        }
    }
}
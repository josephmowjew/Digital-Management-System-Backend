using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.User;

namespace DataStore.Core.Mappers
{
    public class EmailSignatureProfile : Profile
    {
        public EmailSignatureProfile()
        {
            CreateMap<GenericSignature, SignatureDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Tel, opt => opt.MapFrom(src => src.Tel))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(dest => dest.BannerImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Attachments, opt => opt.Ignore());

            CreateMap<SignatureDTO, GenericSignature>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Tel, opt => opt.MapFrom(src => src.Tel))
                .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Mobile))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Website))
                .ForMember(dest => dest.Attachments, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}


using AutoMapper;
using DataStore.Core.DTOs.Signature;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class SignatureProfile : Profile
    {
        public SignatureProfile()
        {
            //add mappings for stamp and stampDTOs
            CreateMap<CreateSignatureDTO, Signature>()
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<Signature, ReadSignatureDTO>();
            CreateMap<UpdateSignatureDTO, Signature>()
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());;
        }
    }
}

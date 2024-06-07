using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.Penalty;

namespace DataStore.Core.Mappers
{
    public class PenaltyProfile : Profile
    {
        public PenaltyProfile()
        {
            CreateMap<Penalty, ReadPenaltyDTO>();
            CreateMap<CreatePenaltyDTO, Penalty>()
             .ForMember(dest => dest.Attachments, opt => opt.Ignore());
            CreateMap<UpdatePenaltyDTO, Penalty>()
            .ForMember(dest => dest.Attachments, opt => opt.Ignore());
        }
    }
}

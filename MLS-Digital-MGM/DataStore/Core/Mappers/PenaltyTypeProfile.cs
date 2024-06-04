using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.PenaltyType;

namespace DataStore.Core.Mappers
{
    public class PenaltyTypeProfile : Profile
    {
        public PenaltyTypeProfile()
        {
            CreateMap<PenaltyType, ReadPenaltyTypeDTO>();
            CreateMap<CreatePenaltyTypeDTO, PenaltyType>();
            CreateMap<UpdatePenaltyTypeDTO, PenaltyType>();
        }
    }
}

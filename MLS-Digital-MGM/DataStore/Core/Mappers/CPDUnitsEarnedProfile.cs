using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.CPDUnitsEarned;

namespace DataStore.Core.Mappers
{
    public class CPDUnitsEarnedProfile : Profile
    {
        public CPDUnitsEarnedProfile()
        {
            CreateMap<CPDUnitsEarned, ReadCPDUnitsEarnedDTO>();
            CreateMap<CreateCPDUnitsEarnedDTO, CPDUnitsEarned>();
            CreateMap<UpdateCPDUnitsEarnedDTO, CPDUnitsEarned>();
        }
    }
}


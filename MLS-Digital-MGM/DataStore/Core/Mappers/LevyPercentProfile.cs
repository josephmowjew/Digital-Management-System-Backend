using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.LevyPercent;

namespace DataStore.Core.Mappers
{
    public class LevyPercentProfile : Profile
    {
        public LevyPercentProfile()
        {
            CreateMap<LevyPercent, ReadLevyPercentDTO>();
            CreateMap<CreateLevyPercentDTO, LevyPercent>();
            CreateMap<UpdateLevyPercentDTO, LevyPercent>();
        }
    }
}

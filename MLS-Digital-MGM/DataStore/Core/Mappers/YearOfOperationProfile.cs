using AutoMapper;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class YearOfOperationProfile : Profile
    {
        public YearOfOperationProfile()
        {
            CreateMap<CreateYearOfOperationDTO, YearOfOperation>();
            CreateMap<YearOfOperation, ReadYearOfOperationDTO>();
            CreateMap<UpdateYearOfOperationDTO, YearOfOperation>();
        }
    }
}

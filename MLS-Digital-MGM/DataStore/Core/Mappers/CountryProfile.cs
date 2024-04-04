using AutoMapper;
using DataStore.Core.DTOs.Country;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class CountryProfile : Profile
    {
        // A constructor that initializes mappings between Data Transfer Objects (DTOs) and the Country entity.
        public CountryProfile()
        {
            CreateMap<CreateCountryDTO, Country>();
            CreateMap<Country, ReadCountryDTO>();
            CreateMap<UpdateCountryDTO, Country>();
        }
    }
}
using AutoMapper;
using DataStore.Core.DTOs.Country;
using DataStore.Core.DTOs.Title;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class TitleProfile : Profile
    {
        public TitleProfile()
        {
            CreateMap<CreateTitleDTO, Title>();
            CreateMap<Title, ReadTitleDTO>();
            CreateMap<UpdateTitleDTO, Title>();
        }
    }
}
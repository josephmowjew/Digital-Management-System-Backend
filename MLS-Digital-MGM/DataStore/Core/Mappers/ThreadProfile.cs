using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs;
using DataStore.Core.DTOs.Thread;

namespace DataStore.Core.Mappers
{
    public class ThreadProfile : Profile
    {
        public ThreadProfile()
        {
            CreateMap<DataStore.Core.Models.Thread, ReadThreadDTO>();
            CreateMap<CreateThreadDTO, DataStore.Core.Models.Thread>();
            CreateMap<UpdateThreadDTO, DataStore.Core.Models.Thread>();
        }
    }
}

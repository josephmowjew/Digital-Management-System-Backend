using AutoMapper;
using DataStore.Core.DTOs.ProBono;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class ProBonoProfile : Profile
    {
        public ProBonoProfile()
        {
            CreateMap<ProBonoApplication, ProBono>();
            CreateMap<ProBono, ReadProBonoDTO>();
            CreateMap<UpdateProBonoDTO, ProBono>();
            
        }
    }
}

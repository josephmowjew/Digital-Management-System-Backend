using AutoMapper;
using DataStore.Core.DTOs.ProBono;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class ProBonoProfile : Profile
    {
        public ProBonoProfile()
        {
            CreateMap<ProBonoApplication, ProBono>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ProBono, ReadProBonoDTO>();
            CreateMap<UpdateProBonoDTO, ProBono>();
            
        }
    }
}

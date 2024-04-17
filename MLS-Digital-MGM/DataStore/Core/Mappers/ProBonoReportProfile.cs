using AutoMapper;
using DataStore.Core.DTOs.ProBonoReport;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class ProBonoReportProfile : Profile
    {
        public ProBonoReportProfile()
        {
           CreateMap<CreateProBonoReportDTO, ProBonoReport>()
           .ForMember(dest => dest.Attachments, opt => opt.Ignore());
           CreateMap<UpdateProBonoReportDTO, ProBonoReport>()
           .ForMember(dest => dest.Attachments, opt => opt.Ignore());
           CreateMap<ProBonoReport, ReadProBonoReportDTO>();
        }
    }
}

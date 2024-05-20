using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.LicenseApprovalHistory;

namespace DataStore.Core.Mappers
{
    public class LicenseApprovalHistoryProfile : Profile
    {
        public LicenseApprovalHistoryProfile()
        {
            CreateMap<LicenseApprovalHistory, ReadLicenseApprovalHistoryDTO>();
            CreateMap<CreateLicenseApprovalHistoryDTO, LicenseApprovalHistory>();
            CreateMap<UpdateLicenseApprovalHistoryDTO, LicenseApprovalHistory>();
        }
    }
}

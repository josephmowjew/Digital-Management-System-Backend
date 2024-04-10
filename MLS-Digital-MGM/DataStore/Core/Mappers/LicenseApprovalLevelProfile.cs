using AutoMapper;
using DataStore.Core.DTOs.LicenseApprovalLevelDTO;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class LicenseApprovalLevelProfile : Profile
    {
        public LicenseApprovalLevelProfile()
        {
            CreateMap<LicenseApprovalLevel, ReadLicenseApprovalLevelDTO>();
            CreateMap<CreateLicenseApprovalLevelDTO, LicenseApprovalLevel>();
            CreateMap<UpdateLicenseApprovalLevelDTO, LicenseApprovalLevel>();
        }
    }
}

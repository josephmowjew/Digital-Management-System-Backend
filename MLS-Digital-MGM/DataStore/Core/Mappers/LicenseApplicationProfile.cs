using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.LicenseApplication;

namespace DataStore.Core.Mappers
{
    public class LicenseApplicationProfile : Profile
    {
        public LicenseApplicationProfile()
        {
            CreateMap<LicenseApplication, ReadLicenseApplicationDTO>();
            CreateMap<CreateLicenseApplicationDTO, LicenseApplication>();
            CreateMap<UpdateLicenseApplicationDTO, LicenseApplication>();
        }
    }
}

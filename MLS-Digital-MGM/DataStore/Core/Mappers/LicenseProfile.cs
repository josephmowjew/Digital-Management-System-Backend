using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.License;

namespace DataStore.Core.Mappers
{
    public class LicenseProfile : Profile
    {
        public LicenseProfile()
        {
            CreateMap<License, ReadLicenseDTO>();
           
        }
    }
}

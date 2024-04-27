using AutoMapper;
using DataStore.Core.DTOs.User; 
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
  public class UserProfile : Profile
  {
    public UserProfile() 
    {
   CreateMap<ApplicationUser, ReadUserDTO>()
    .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CreatedDate)))
    .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.LastLogin)))
    .ForMember(dest => dest.IdentityExpiryDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.IdentityExpiryDate)))
    .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)));
      CreateMap<UpdateUserDTO, ApplicationUser>();
    }
  }
}

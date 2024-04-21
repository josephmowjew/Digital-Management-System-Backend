using AutoMapper;
using DataStore.Core.DTOs.User; 
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
  public class UserProfile : Profile
  {
    public UserProfile() 
    {
      CreateMap<ApplicationUser, ReadUserDTO>(); 
      CreateMap<UpdateUserDTO, ApplicationUser>();
    }
  }
}

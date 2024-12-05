using DataStore.Core.DTOs.ApplicationUserChangeRequest;
using DataStore.Core.Models;
using AutoMapper;

namespace DataStore.Core.Mappers;

public class ApplicationUserChangeRequestProfile : Profile
{
    public ApplicationUserChangeRequestProfile()
    {
        CreateMap<ApplicationUserChangeRequest, ReadApplicationUserChangeRequestDto>();
        
        CreateMap<CreateApplicationUserChangeRequestDto, ApplicationUserChangeRequest>();
        
        CreateMap<UpdateApplicationUserChangeRequestDto, ApplicationUserChangeRequest>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
} 
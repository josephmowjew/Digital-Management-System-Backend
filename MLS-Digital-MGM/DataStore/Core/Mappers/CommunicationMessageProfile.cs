using AutoMapper;
using DataStore.Core.Models;
using MLS_Digital_Management_System_Front_End.Core.DTOs.Communication;
using System.Text.Json;

public class CommunicationMessageProfile : Profile
{
    public CommunicationMessageProfile()
    {
        CreateMap<CommunicationMessage, ReadCommunicationMessageDTO>()
            .ForMember(dest => dest.TargetedRoles, opt => opt.MapFrom(src => 
                JsonSerializer.Deserialize<List<string>>(src.TargetedRolesJson ?? "[]", (JsonSerializerOptions)null)))
            .ForMember(dest => dest.TargetedDepartments, opt => opt.MapFrom(src => 
                JsonSerializer.Deserialize<List<string>>(src.TargetedDepartmentsJson ?? "[]", (JsonSerializerOptions)null)));

        
    }
}
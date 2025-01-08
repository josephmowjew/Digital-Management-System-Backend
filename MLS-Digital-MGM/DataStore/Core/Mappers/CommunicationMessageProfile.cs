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
                DeserializeJson<List<string>>(src.TargetedRolesJson)))
            .ForMember(dest => dest.TargetedDepartments, opt => opt.MapFrom(src => 
                DeserializeJson<List<string>>(src.TargetedDepartmentsJson)));
    }

    private T DeserializeJson<T>(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json ?? "[]", (JsonSerializerOptions)null);
        }
        catch (JsonException ex)
        {
            // Log the error (consider using a logging framework)
            Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
            return default; // or handle as needed
        }
    }
}
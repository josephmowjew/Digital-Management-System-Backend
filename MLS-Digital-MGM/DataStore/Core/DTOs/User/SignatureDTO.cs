namespace DataStore.Core.DTOs.User;

using DataStore.Helpers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class SignatureDTO
{
    public SignatureDTO()
    {
        Attachments = new List<IFormFile>();
    }
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string CompanyName { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Tel { get; set; }
    [Required]
    public string Mobile { get; set; }
    [Required]
    public string Website { get; set; }

    public bool IsActive { get; set; }

    [JsonIgnore]
    [AllowedFileTypes(new[] { ".png", ".jpg", ".jpeg" })]
    [FileSize(5242880)] // 5 MB
    public List<IFormFile>? Attachments { get; set; }

    public string? BannerImageUrl { get; set; }
}

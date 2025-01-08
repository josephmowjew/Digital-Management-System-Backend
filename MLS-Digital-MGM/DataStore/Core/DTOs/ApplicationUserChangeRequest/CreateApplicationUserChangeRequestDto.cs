using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.ApplicationUserChangeRequest
{
    public class CreateApplicationUserChangeRequestDto
    {
        [Required]
        public string UserId { get; set; }

        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
    }
}

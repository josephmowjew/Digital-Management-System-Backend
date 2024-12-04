using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.ApplicationUserChangeRequest
{
    public class CreateApplicationUserChangeRequestDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

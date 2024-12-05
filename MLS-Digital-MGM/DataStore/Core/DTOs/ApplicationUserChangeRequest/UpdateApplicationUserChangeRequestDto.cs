using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.ApplicationUserChangeRequest
{
    public class UpdateApplicationUserChangeRequestDto
    {
        [Required]
        public int Id { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Status { get; set; }
    }
}

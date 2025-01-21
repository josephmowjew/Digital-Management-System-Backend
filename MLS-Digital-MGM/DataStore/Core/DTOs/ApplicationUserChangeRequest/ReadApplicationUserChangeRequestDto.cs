using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.DTOs.User;

namespace DataStore.Core.DTOs.ApplicationUserChangeRequest
{
    public class ReadApplicationUserChangeRequestDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public string Status { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ReadUserDTO User { get; set; }
    }
}

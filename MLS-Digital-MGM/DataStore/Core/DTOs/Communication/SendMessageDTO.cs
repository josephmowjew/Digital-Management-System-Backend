using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.Communication
{
    public class SendMessageDTO
    {
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public List<int> DepartmentIds { get; set; } = new List<int>();

        public List<string> RoleNames { get; set; } = new List<string>();

        public bool SendToAllUsers { get; set; }
        
        public List<IFormFile>? Attachments { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.SubcommitteeMessage
{
    public class UpdateSubcommitteeMessageDTO
    {
        public int Id { get; set; }

        public int SubcommitteeID { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [StringLength(300)]
        public string Content { get; set; }

        public List<IFormFile>? Attachments { get; set; }

        public int? SubcommitteeThreadId { get; set; }
    }
}

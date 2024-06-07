using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.Message
{
    public class CreateMessageDTO
    {
        public int CommitteeID { get; set; }

        public int SenderID { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [StringLength(maximumLength:300)]
        public string Content { get; set; }

        public List<IFormFile> Attachments { get; set; }

        public int? ThreadID { get; set; }
    }
}

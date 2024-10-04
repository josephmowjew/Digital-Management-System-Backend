using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DataStore.Helpers;

namespace DataStore.Core.DTOs.NotaryPublic
{
    public class CreateNotaryPublicDTO
    {
        public CreateNotaryPublicDTO()
        {
            Attachments = new List<IFormFile>();
        }
        public int MemberId { get; set; }
        public string ApplicationStatus { get; set; } = Lambda.Pending;
        public DateTime? ApprovedDate { get; set; } = null;
        public DateTime? DeniedDate { get; set; } = null;
        [Required]
        public int YearOfOperationId { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}

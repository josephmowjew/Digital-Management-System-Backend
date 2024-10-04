
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.NotaryPublic
{
    public class UpdateNotaryPublicDTO
    {
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int YearOfOperationId { get; set; }
        public string ApplicationStatus { get; set; } = Lambda.Pending;
        public DateTime? ApprovedDate { get; set; } = null;
        public DateTime? DeniedDate { get; set; } = null;

        public List<IFormFile>? Attachments { get; set; }
    }
}


using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.Stamp
{
    public class UpdateStampDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        public int YearOfOperationId { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}


using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.Models;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.Stamp
{
    public class CreateStampDTO
    {
        public CreateStampDTO()
        {
            Attachments = new List<IFormFile>();
        }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }

        [Required]
        public int YearOfOperationId { get; set; }

        [Required]
        public List<IFormFile>? Attachments { get; set; }
    }
}

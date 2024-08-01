using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.CPDQualification
{
    public class CreateCPDQualificationDTO
    {
        [Required]
        public int CPDTrainingId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public DateTime DateGenerated { get; set; }

        [FileSize(5242880)] // 5 MB
        [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt" })]
        public ICollection<IFormFile>? Attachments { get; set; } 
    }
}

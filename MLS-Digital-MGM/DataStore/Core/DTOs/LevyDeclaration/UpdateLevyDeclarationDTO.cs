using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.LevyDeclaration
{
    public class UpdateLevyDeclarationDTO
    {
        public int Id { get; set; }
        [Required]
        public DateTime Month { get; set; }
        [Required]
        public decimal Revenue { get; set; }
        public int FirmId { get; set; }
        [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", "png", "jpg", "jpeg" })]
        [FileSize(5242880)] // 5 MB
        public List<IFormFile>? Attachments { get; set; }
    }
}

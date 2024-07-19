using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.LevyDeclaration
{
    public class CreateLevyDeclarationDTO
    {
        public CreateLevyDeclarationDTO()
        {
            Attachments = new List<IFormFile>();
        }
        public DateTime Month { get; set; }
        public int FirmId { get; set; }
        public decimal Revenue { get; set; }

        [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", "png", "jpg", "jpeg" })]
        [FileSize(5242880)] // 5 MB
        public List<IFormFile>? Attachments { get; set; }
    }
}

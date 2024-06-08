using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.DTOs.Attachment;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.PenaltyPayment
{
    public class CreatePenaltyPaymentDTO
    {
        public CreatePenaltyPaymentDTO()
        {
            Attachments = new List<IFormFile>();
        }
        public int Id { get; set; }
        public int PenaltyId { get; set; }
        [Required]
        public double Fee { get; set; }
        [StringLength(250)]
        public string? Description { get; set; }

        [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", "png", "jpg", "jpeg" })]
        [FileSize(5242880)] // 5 MB
        public List<IFormFile>? Attachments { get; set; }
    }
}

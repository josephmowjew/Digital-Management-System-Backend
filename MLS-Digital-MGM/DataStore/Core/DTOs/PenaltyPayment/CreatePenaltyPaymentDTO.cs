using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.PenaltyPayment
{
    public class CreatePenaltyPaymentDTO
    {
        public int PenaltyId { get; set; }

        [Required]
        [Display(Name = "Payment Fee")]
        public double Fee { get; set; }

        [StringLength(maximumLength: 250)]
        public string? Description { get; set; }

        [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", "png","jpg","jpeg" })]
        [FileSize(5242880)] // 5 MB
        public ICollection<IFormFile>? Attachments { get; set; }
    }
}

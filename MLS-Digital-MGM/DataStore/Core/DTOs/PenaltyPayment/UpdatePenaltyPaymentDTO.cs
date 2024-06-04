using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.Models;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.PenaltyPayment
{
    public class UpdatePenaltyPaymentDTO
    {
        public int Id { get; set; }

        [Display(Name = "Penalty Identification Number")]
        public int PenaltyId { get; set; }

        [Required]
        public double Fee { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        public string? PaymentStatus { get; set; }

        [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt","png","jpg","jpeg" })]
        [FileSize(5242880)] // 5 MB
        public ICollection<IFormFile>? Attachments { get; set; }
    }
}

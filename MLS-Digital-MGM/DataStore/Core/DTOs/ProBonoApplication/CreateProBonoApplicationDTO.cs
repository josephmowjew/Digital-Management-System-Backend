using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.Models;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.ProBonoApplication
{
    public class CreateProBonoApplicationDTO
    {
        [Required, MaxLength(200)]
        public string NatureOfDispute { get; set; }

        [Required]
        public string CaseDetails { get; set; }

        public int ProbonoClientId { get; set; }

        public string ApplicationStatus { get; set; } = Lambda.Pending;

        public DateTime? ApprovedDate { get; set; } = null;

    
        public string SummaryOfDispute { get; set; }


        public int YearOfOperationId { get; set; }
        [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt" })]
        [FileSize(5242880)] // 5 MB
        public ICollection<IFormFile>? Attachments { get; set; }  

    }
}

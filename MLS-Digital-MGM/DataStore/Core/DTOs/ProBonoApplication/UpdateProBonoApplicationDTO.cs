using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.Models;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.ProBonoApplication
{
    public class UpdateProBonoApplicationDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string NatureOfDispute { get; set; }

        [Required]
        public string CaseDetails { get; set; }

        public int ProbonoClientId { get; set; }

        public string ApplicationStatus { get; set; }

        [StringLength(200)]
        public string DenialReason { get; set; }

        public string SummaryOfDispute { get; set; }

        public ICollection<IFormFile> Attachments { get; set; } = new List<IFormFile>(); 
    }
}

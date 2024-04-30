using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.Models;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.ProBonoReport
{
    public class UpdateProBonoReportDTO
    {
        public int Id { get; set; }
        [Display(Name = "Probono Identification Number")]
        public int ProBonoId { get; set; }

        [Required]
        [Display(Name = "Report Proposed Hours")]
        public double ProBonoProposedHours { get; set; }

        public double ProBonoHours { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public string? ReportStatus { get; set; }


        public ICollection<IFormFile>? Attachments { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.ProBonoReport
{
    public class CreateProBonoReportDTO
    {
        [Display(Name = "Probono Identification Number")]
        public int ProBonoId { get; set; }

        [Required]
        [Display(Name = "Report Proposed Hours")]
        public double ProBonoProposedHours { get; set; }
        [StringLength(maximumLength: 250)]
        public string? Description { get; set; }
        
        //public string ReportStatus { get; set; } = "Pending";

        public ICollection<IFormFile> Attachments { get; set; }  
    }
}

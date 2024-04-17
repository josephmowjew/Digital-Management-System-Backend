using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.ProBonoReport
{
    public class ReadProBonoReportDTO
    {
        public int Id { get; set; }

        public int ProBonoId { get; set; }

        public double ProBonoProposedHours { get; set; }

        public double ProBonoHours { get; set; }

        public string ReportStatus { get; set; }

        public string ApprovedById { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public ICollection<Attachment> Attachments { get; set; }
    }
}

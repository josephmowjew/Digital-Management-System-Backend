using DataStore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class ProBonoReport: Meta
    {
        public ProBonoReport()
        {
            Attachments = new List<Attachment>();
        }
        public int ProBonoId { get; set; }
        public ProBono ProBono { get; set; }
        public double ProBonoProposedHours { get; set; }
        public double ProBonoHours {  get; set; }
        public string ReportStatus { get; set; } = Lambda.Pending;
        public string? ApprovedById { get; set; }
        public ApplicationUser ApprovedBy { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        [StringLength(maximumLength: 250)]
        public string Description { get; set; }
        public List<Attachment> Attachments { get; set; }  
    }
}

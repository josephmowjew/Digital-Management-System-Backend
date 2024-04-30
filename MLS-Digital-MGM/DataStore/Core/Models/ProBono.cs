using DataStore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class ProBono: Meta
    {
        public ProBono()
        {
            Attachments = new List<Attachment>();
            Members = new List<Member>();
            ProBonoReports = new List<ProBonoReport>();
        }
        [Required]
        [StringLength(maximumLength:100)]
        public string FileNumber { get; set; }
        [Required, MaxLength(200)]
        public string NatureOfDispute { get; set; }
        [Required]
        public string CaseDetails { get; set; }
        public string? CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public int ProbonoClientId { get; set; }
        public ProbonoClient ProbonoClient { get; set; }
        public int ProBonoApplicationId { get; set; }
        public ProBonoApplication ProBonoApplication { get; set; }
        public string SummaryOfDispute { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }
        public ICollection<Member> Members { get; set; }
        public ICollection<ProBonoReport> ProBonoReports { get; set; }
    }
}

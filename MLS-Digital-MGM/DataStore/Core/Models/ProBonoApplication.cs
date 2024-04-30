using DataStore.Core.Models.Interfaces;
using DataStore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class ProBonoApplication: Meta, IOwned
    {
        public ProBonoApplication()
        {
            Attachments = new List<Attachment>();
        }
        [Required, MaxLength(200)]
        public string NatureOfDispute { get; set; }
        [Required]
        public string CaseDetails { get; set; }
        public string? CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public int ProbonoClientId { get; set; }
        public ProbonoClient ProbonoClient { get; set; }
        public string ApplicationStatus { get; set; } = Lambda.Pending;
        public DateTime? ApprovedDate { get; set; }
        [StringLength(200)]
        public string? DenialReason { get; set; }
        public string SummaryOfDispute { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }
    }
}

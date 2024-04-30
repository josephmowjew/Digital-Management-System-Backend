using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.ProBonoApplication
{
    public class ReadProBonoApplicationDTO
    {
        public ReadProBonoApplicationDTO()
        {
            Attachments = new List<DataStore.Core.Models.Attachment>();
        }

        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string NatureOfDispute { get; set; }

        [Required]
        public string CaseDetails { get; set; }

        public string CreatedById { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public int ProbonoClientId { get; set; }

        public ProbonoClient ProbonoClient { get; set; }

        public string ApplicationStatus { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string DenialReason { get; set; }

        public string SummaryOfDispute { get; set; }
        public int YearOfOperationId { get; set; }
        public DataStore.Core.Models.YearOfOperation YearOfOperation { get; set; }
        public ICollection<DataStore.Core.Models.Attachment> Attachments { get; set; }
        public DateOnly CreatedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Helpers;

namespace DataStore.Core.Models
{
    public class NotaryPublic : Meta
    {
        public NotaryPublic()
        {
            Attachments = new List<Attachment>();
        }

        public int MemberId { get; set; }
        public Member Member { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string ApplicationStatus { get; set; } = Lambda.Pending;
        public DateTime? ApprovedDate { get; set; }
        [StringLength(200)]
        public string? DenialReason { get; set; }
        public DateTime? DeniedDate { get; set; }
        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}

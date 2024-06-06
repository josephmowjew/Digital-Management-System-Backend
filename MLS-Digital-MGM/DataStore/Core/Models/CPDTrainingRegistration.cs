using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class CPDTrainingRegistration: Meta
    {
        public CPDTrainingRegistration()
        {
            Attachments = new List<Attachment>();
        }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        [StringLength(100)]
        public string RegistrationStatus { get; set; }
        public double Fee { get; set; }
        public CPDTraining CPDTraining { get; set; }
        public int CPDTrainingId { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        [StringLength(250)]
        public string? DeniedReason { get; set; }
        public virtual List<Attachment> Attachments { get; set; }
        public DateTime? DateOfPayment { get; set; }

       
    }
}

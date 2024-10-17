using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class CPDTraining: Meta
    {
        public CPDTraining()
        {
            this.CPDUnitsEarned = new List<CPDUnitsEarned>();
            this.CPDTrainingRegistration = new List<CPDTrainingRegistration>();
            this.Attachments = new List<Attachment>();

        }

        [StringLength(maximumLength:100)]
        [Required]
        public string Title { get; set; }
        [StringLength(maximumLength:250)]
        public string Description { get; set; }
        public double Duration { get; set; }
        public DateTime DateToBeConducted { get; set; }
        [StringLength(maximumLength:250)]
        public string? PhysicalVenue { get; set; }
        public string ApprovalStatus { get; set; }
        public int ProposedUnits { get; set; }
        public double? MemberPhysicalAttendanceFee { get; set; } 
        public double? MemberVirtualAttendanceFee { get; set; } 
        public double? NonMemberPhysicalAttendanceFee { get; set; } 
         public double? NonMemberVirtualAttandanceFee { get; set; }
        public DateTime RegistrationDueDate { get; set; }
        public bool IsFree { get; set; } = false;
        public int CPDUnitsAwarded { get; set; }
        [StringLength(maximumLength:200)]
        public string? AccreditingInstitution { get; set; }
        public string? AccreditingInstitutionRepresentativePosition { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }
        public virtual List<CPDUnitsEarned> CPDUnitsEarned { get; set; }
        public virtual List<CPDTrainingRegistration> CPDTrainingRegistration { get; set;}

        public List<Attachment> Attachments { get; set; }

        //public List<Attachment> Signature { get; set; }
    }
}

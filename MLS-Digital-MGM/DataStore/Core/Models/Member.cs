using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class Member : Meta
    {
       
       public Member()
       {
           this.QualificationTypes = new List<QualificationType>();
           this.ProBonos = new List<ProBono>();
           this.CPDTrainingRegistrations = new List<CPDTrainingRegistration>();
       }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required, MaxLength(250)]
        public string PostalAddress { get; set; }
        [Required, MaxLength(250)]
        public string PermanentAddress { get; set; }
        [Required, MaxLength(250)]
        public string ResidentialAddress { get; set; }
        [Required]
        public DateTime DateOfAdmissionToPractice { get; set; }
        public virtual List<QualificationType> QualificationTypes { get; set; }
        public ICollection<ProBono> ProBonos { get; set; }
        public int? FirmId { get; set; }
        public Firm Firm { get; set; }
        public virtual List<CPDTrainingRegistration> CPDTrainingRegistrations { get; set; }

    }
}

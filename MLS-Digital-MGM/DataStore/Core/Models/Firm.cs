using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class Firm : Meta
    {
        public Firm()
        {
            this.Users = new List<ApplicationUser>();
        }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 250)]
        public string PostalAddress { get; set; }
        [Required]
        [StringLength(maximumLength: 250)]
        public string PhysicalAddress { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string PrimaryContactPerson { get; set; }
        [Required]
        [StringLength(maximumLength: 15)]
        public string PrimaryPhoneNumber { get; set; }
        [StringLength(maximumLength: 100)]
        public string SecondaryContactPerson { get; set; }
        [StringLength(maximumLength: 15)]
        public string SecondaryPhoneNumber { get; set; }
        public string? CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }
        [StringLength(maximumLength: 250)]
        public string? DenialReason { get; set; }
        public string? CustomerId { get; set; }
        public QBCustomer Customer { get; set; }
        public int? InstitutionTypeId { get; set; }
        public virtual InstitutionType InstitutionType { get; set; }
    }
}

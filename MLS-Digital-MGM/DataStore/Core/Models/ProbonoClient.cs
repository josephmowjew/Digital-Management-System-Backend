using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.Models.Interfaces;

namespace DataStore.Core.Models
{
    public class ProbonoClient: Meta
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength:200)]
        public string NationalId { get; set; }
        [Required, MaxLength(250)]
        public string PostalAddress { get; set; }
        [Required, MaxLength(250)]
        public string PermanentAddress { get; set; }
        [Required, MaxLength(250)]
        public string ResidentialAddress { get; set; }
        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }
        [MaxLength(150)]
        public string OtherContacts { get; set; }
        public DateTime ApprovedDate { get; set; }

        [Required, MaxLength(150)]
        public string Occupation { get; set; }
        [Required, Range(0, double.MaxValue)]
        public decimal AnnualIncome { get; set; }
        public ApplicationUser CreatedBy { get ; set ; }
        public string CreatedById { get; set; }
        public bool deleteRequest {get; set;} = false;
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs.Firms
{
    public class CreateFirmDTO
    {
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
    }
}

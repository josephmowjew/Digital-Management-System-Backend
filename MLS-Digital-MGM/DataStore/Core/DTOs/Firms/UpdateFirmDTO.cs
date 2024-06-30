// UpdateFirmDTO.cs
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Firms
{
    public class UpdateFirmDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string PostalAddress { get; set; }

        [Required]
        [StringLength(250)]
        public string PhysicalAddress { get; set; }

        [Required]
        [StringLength(100)]
        public string PrimaryContactPerson { get; set; }

        [Required]
        [StringLength(15)]
        public string PrimaryPhoneNumber { get; set; }

        [StringLength(100)]
        public string SecondaryContactPerson { get; set; }

        [StringLength(15)]
        public string SecondaryPhoneNumber { get; set; }
        public string? CustomerId {get; set;}

    }
}
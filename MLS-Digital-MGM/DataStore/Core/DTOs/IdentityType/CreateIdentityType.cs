using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.IdentityType
{
    public class CreateIdentityTypeDTO
    {
        [Required]
        [StringLength(maximumLength: 150)]
        public string Name { get; set; }
        // Add more properties as needed
    }
}
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.IdentityType
{
    public class UpdateIdentityTypeDTO
    {
        // Define properties here to represent the data fields of the DTO
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:150)]
        public string Name { get; set; }
        // Add more properties as needed
    }
}
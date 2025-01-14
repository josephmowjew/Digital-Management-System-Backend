using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.InstitutionType
{
    public class CreateInstitutionTypeDTO
    {
        [Required]
        [StringLength(maximumLength: 150)]
        public string Name { get; set; }
        // Add more properties as needed
    }
}
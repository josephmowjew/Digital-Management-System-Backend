using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.PenaltyType
{
    public class CreatePenaltyTypeDTO
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 250)]
        public string Description { get; set; }
    }
}

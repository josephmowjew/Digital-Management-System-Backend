using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Committee
{
    public class CreateCommitteeDTO
    {
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string CommitteeName { get; set; }

        [StringLength(maximumLength: 500)]
        public string Description { get; set; }

        public int? ChairpersonID { get; set; }
         public int YearOfOperationId { get; set; }
    }
}

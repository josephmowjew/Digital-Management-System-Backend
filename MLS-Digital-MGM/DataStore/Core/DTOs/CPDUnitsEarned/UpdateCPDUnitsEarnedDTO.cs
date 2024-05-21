using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.CPDUnitsEarned
{
    public class UpdateCPDUnitsEarnedDTO
    {
        public int Id { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int CPDTrainingId { get; set; }
        [Required]
        public int UnitsEarned { get; set; }
        [Required]
        public int YearOfOperationId { get; set; }
    }
}

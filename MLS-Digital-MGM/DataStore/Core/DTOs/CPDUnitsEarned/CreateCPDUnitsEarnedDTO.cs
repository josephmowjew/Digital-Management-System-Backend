using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.CPDUnitsEarned
{
    public class CreateCPDUnitsEarnedDTO
    {
        public int MemberId { get; set; }
        public int CPDTrainingId { get; set; }
        [Required]
        public int UnitsEarned { get; set; }
        public int YearOfOperationId { get; set; }
    }
}

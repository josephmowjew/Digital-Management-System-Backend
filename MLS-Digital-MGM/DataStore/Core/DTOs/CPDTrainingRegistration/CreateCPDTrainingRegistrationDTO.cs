using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.CPDTrainingRegistration
{
    public class CreateCPDTrainingRegistrationDTO
    {
        public int MemberId { get; set; }
        [StringLength(100)]
        public string RegistrationStatus { get; set; }
        public int CPDTrainingId { get; set; }
        public string CreatedById { get; set; }
    }
}

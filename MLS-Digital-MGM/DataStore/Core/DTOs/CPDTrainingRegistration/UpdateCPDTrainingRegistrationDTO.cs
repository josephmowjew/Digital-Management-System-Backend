using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.CPDTrainingRegistration
{
    public class UpdateCPDTrainingRegistrationDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        [Required]
        [StringLength(100)]
        public string RegistrationStatus { get; set; }
        public int CPDTrainingId { get; set; }
        [Required]
        public string CreatedById { get; set; }
        public List<IFormFile?> Attachments { get; set; }
        public DateTime? DateOfPayment { get; set; }
    }
}

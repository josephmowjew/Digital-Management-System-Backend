using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.CPDTrainingRegistration
{
    public class CreateCPDTrainingRegistrationDTO
    {
        public CreateCPDTrainingRegistrationDTO()
        {
            Attachments = new List<IFormFile>();
        }
        [StringLength(100)]
        public string CPDTrainingId { get; set; }
        public List<IFormFile>? Attachments { get; set; }
        public DateTime? DateOfPayment { get; set; }
    }
}

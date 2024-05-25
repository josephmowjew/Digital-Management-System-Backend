using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.CPDTrainingRegistration
{
    public class CreateCPDTrainingRegistrationDTO
    {

        [StringLength(100)]
        public string CPDTrainingId { get; set; }
        public List<IFormFile?> Attachments { get; set; }
    }
}

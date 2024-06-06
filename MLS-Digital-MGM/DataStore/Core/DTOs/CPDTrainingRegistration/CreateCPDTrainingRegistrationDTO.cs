using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;
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
        [Display(Name ="Date of payment")]
        [DateLessThanOrEqualToToday(ErrorMessage = "Date of payment can not be of a future date.")]
        public DateTime? DateOfPayment { get; set; }
        public string AttendanceMode { get; set; }
    }
}

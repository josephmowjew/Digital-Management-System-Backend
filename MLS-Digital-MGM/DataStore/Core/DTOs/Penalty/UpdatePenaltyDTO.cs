using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.Penalty
{
    public class UpdatePenaltyDTO
    {
        public int Id { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int PenaltyTypeId { get; set; }
        public double Fee { get; set; }
        [Required]
        [StringLength(250)]
        public string Reason { get; set; }
        [Required]
        public string CreatedById { get; set; }
        public int YearOfOperationId { get; set; }
        [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt","png","jpg","jpeg" })]
        [FileSize(5242880)] // 5 MB
        public List<IFormFile>? Attachments { get; set; }
    }
}

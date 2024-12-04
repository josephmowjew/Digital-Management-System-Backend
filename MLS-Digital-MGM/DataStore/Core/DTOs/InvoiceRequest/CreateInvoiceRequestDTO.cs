using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.InvoiceRequest
{
    public class CreateInvoiceRequestDTO
    {
        [Required]
        public double Amount { get; set; }
        public string? CustomerId { get; set; }
        [Required]
        public string ReferencedEntityType { get; set; }
        [Required]
        public string ReferencedEntityId { get; set; }
        public string? Description { get; set; }
        public IFormFile? Attachment { get; set; }
        public string RequestType { get; set; } = "Individual";
        public string? FirmMembers { get; set; }
    }
}

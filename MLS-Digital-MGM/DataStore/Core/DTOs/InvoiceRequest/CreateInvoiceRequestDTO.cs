using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.InvoiceRequest
{
    public class CreateInvoiceRequestDTO
    {
        //public int InvoiceRequestTypeId { get; set; }
        //public string? CreatedById { get; set; }
        [Required]
        public double Amount { get; set; }
        // public string CustomerId { get; set; }
        // Polymorphic association properties
        public string ReferencedEntityType { get; set; }
        public string ReferencedEntityId { get; set; }
    }
}

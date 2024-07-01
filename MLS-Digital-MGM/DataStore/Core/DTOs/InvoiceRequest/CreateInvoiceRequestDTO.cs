using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.InvoiceRequest
{
    public class CreateInvoiceRequestDTO
    {
        [Required]
        public double? Amount { get; set; }
        public string ReferencedEntityType { get; set; }

        public string Description { get; set; }
        public string ReferencedEntityId { get; set; }
    }
}

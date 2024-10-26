using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.InvoiceRequest
{
    public class UpdateInvoiceRequestDTO
    {
        public int Id { get; set; }
        
        [Required]
        public string CreatedById { get; set; }
        public double Amount { get; set; }
        public string ReferencedEntityType { get; set; }
        public string ReferencedEntityId { get; set; }
    }
}

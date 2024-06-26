using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.InvoiceRequestType
{
    public class CreateInvoiceRequestTypeDTO
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
    }
}

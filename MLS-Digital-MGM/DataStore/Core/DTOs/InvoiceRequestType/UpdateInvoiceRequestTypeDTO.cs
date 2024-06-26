using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.InvoiceRequestType
{
    public class UpdateInvoiceRequestTypeDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}

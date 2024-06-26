using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.InvoiceRequestType
{
    public class ReadInvoiceRequestTypeDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }

    }
}

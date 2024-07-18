using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.Attachment;
using DataStore.Core.DTOs.Firms;
using DataStore.Core.DTOs.InvoiceRequest;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.LevyDeclaration
{
    public class ReadLevyDeclarationDTO
    {
        public ReadLevyDeclarationDTO()
        {
            // Initialize any collections if needed
        }

        public int Id { get; set; }
        public DateTime Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal LevyAmount { get; set; }
        public decimal Percentage { get; set; } // The percentage used for this calculation
        public int FirmId { get; set; }
        public ReadFirmDTO Firm { get; set;}
        public int? InvoiceRequestId { get; set; }
        public ReadInvoiceRequestDTO InvoiceRequest { get; set; }
        public List<ReadAttachmentDTO> Attachments { get; set; }
    }
}

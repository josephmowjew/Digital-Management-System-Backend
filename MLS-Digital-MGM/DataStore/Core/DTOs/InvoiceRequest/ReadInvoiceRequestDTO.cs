using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.Attachment;
using DataStore.Core.DTOs.InvoiceRequestType;
using DataStore.Core.DTOs.User;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;
using DataStore.Core.DTOs.Member;
using DataStore.Core.DTOs.Firms;

namespace DataStore.Core.DTOs.InvoiceRequest
{
    public class ReadInvoiceRequestDTO
    {
        public int Id { get; set; }
       
        public string CreatedById { get; set; }
        public ReadUserDTO CreatedBy { get; set; }
        public double Amount { get; set; }
        public string CustomerId { get; set; }
        public QBCustomer Customer { get; set; }
        public string Status { get; set; }
        public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation { get; set; }
        // Polymorphic association properties
        public string ReferencedEntityType { get; set; }
        public string ReferencedEntityId { get; set; }

        public object ReferencedEntity { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? QBInvoiceId { get; set; }
        public QBInvoice QBInvoice { get; set; }
        public string? InvoiceNumber { get; set; }
        public int? AttachmentId { get; set; }
        public ReadAttachmentDTO Attachment { get; set; }
        public string RequestType { get; set; }
        public String? FirmMembers { get; set; }
        public int? FirmId { get; set; }
        public ReadFirmDTO Firm { get; set; }
    }
}

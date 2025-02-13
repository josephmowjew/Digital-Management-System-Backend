using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Helpers;

namespace DataStore.Core.Models
{
    public class InvoiceRequest: Meta
    {

        public string? CreatedById { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        [Required]
        public double? Amount { get; set; }
        public string? CustomerId { get; set; }

        public QBCustomer Customer { get; set; }

        [StringLength(maximumLength:100)]
        public string Status { get; set; } = Lambda.Pending;
        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }
        // Polymorphic association properties
        public string ReferencedEntityType { get; set; }
        public string ReferencedEntityId { get; set; }

        [StringLength(maximumLength:250)]
        public string? Description { get; set; }
        public string? QBInvoiceId { get; set; }
        public QBInvoice QBInvoice { get; set; }
        public string? InvoiceNumber { get; set; }

        public int? AttachmentId { get; set; }
        public virtual Attachment Attachment { get; set; }
        public int? FirmId { get; set; }
        public virtual Firm Firm { get; set; }

        [StringLength(maximumLength:50)]
        public string RequestType { get; set; } = "Individual";
        
        [StringLength(maximumLength:4000)]
        public string? FirmMembers { get; set; }
    }
}
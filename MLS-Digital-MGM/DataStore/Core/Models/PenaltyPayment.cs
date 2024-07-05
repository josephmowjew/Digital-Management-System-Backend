using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;

namespace DataStore.Core.Models
{
    public class PenaltyPayment : Meta
    {
        public int PenaltyId { get; set; }
        public Penalty Penalty { get; set; }
        public string PaymentStatus { get; set; } = Lambda.Pending;
        public string? Description { get; set; }
        public double Fee { get; set; }
        public List<Attachment> Attachments { get; set; }
        public DateTime? DateApproved { get; set; }

        public DateTime? DateDenied { get; set; }
        [StringLength(250)]
        public string? ReasonForDenial { get; set; }
        public string? QBInvoiceId { get; set; }
        public QBInvoice QBInvoice { get; set; }

    }
}

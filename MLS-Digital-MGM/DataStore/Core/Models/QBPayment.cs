using DataStore.Core.Models.Interfaces;
using DataStore.Helpers;
using Microsoft.AspNetCore.Identity;
using System;

namespace DataStore.Core.Models
{
    public class QBPayment : Meta
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public QBCustomer Customer { get; set; }
        public string InvoiceId { get; set; }
        public QBInvoice Invoice { get; set; } 
        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}

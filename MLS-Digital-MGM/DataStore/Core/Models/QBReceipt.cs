using DataStore.Core.Models.Interfaces;
using DataStore.Helpers;
using Microsoft.AspNetCore.Identity;
using System;

namespace DataStore.Core.Models
{
    public class QBReceipt : Meta
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public QBCustomer Customer { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public string PaymentId { get; set; }
        public string InvoiceId { get; set; }
        public QBInvoice Invoice { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

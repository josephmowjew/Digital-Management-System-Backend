using System;
using DataStore.Core.Models;

namespace DataStore.Core.ViewModels
{
    public class QBReceiptViewModel
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public string PaymentId { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

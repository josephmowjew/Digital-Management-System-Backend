using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs.QBInvoicesDTOs
{
    public class ReadQBInvoiceDTO
    {
        public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerId { get; set; }
        public DataStore.Core.DTOs.QBCustomerDTOs.ReadQBCustomerDTO Customer { get; set; }
        public string CustomerName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal UnpaidAmount { get; set; }
        public string InvoiceType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string InvoiceDescription { get; set; }
    }
}

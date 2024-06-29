using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.InvoiceRequestType;
using DataStore.Core.DTOs.User;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;

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

        public DateTime CreatedDate { get; set; }

    }
}

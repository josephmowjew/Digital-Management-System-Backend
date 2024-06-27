using DataStore.Core.Models.Interfaces;
using DataStore.Helpers;
using Microsoft.AspNetCore.Identity;
using System;

namespace DataStore.Core.Models
{
    public class QBCustomer : Meta
    {
        
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingAddressLine3 { get; set; }
        public string BillingAddressLine4 { get; set; }
        public string BillingAddressLine5 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Province { get; set; }
        public bool ActiveStatus { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public decimal AccountBalance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

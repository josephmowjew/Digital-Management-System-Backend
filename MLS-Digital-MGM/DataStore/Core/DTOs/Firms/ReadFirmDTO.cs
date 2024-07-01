using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.Firms
{
    public class ReadFirmDTO
    {
        public ReadFirmDTO()
        {
            Users = new List<ApplicationUser>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string PostalAddress { get; set; }

        public string PhysicalAddress { get; set; }

        public string PrimaryContactPerson { get; set; }

        public string PrimaryPhoneNumber { get; set; }

        public string SecondaryContactPerson { get; set; }

        public string SecondaryPhoneNumber { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
        public string Status {get; set;}

         public string? CustomerId {get; set;}
        public QBCustomer Customer {get; set;}
    }
}

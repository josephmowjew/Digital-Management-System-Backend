using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs.ProBonoClient
{
    public class ReadProBonoClientDTO
    {

        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string NationalId { get; set; }

        [Required, MaxLength(250)]
        public string PostalAddress { get; set; }

        [Required, MaxLength(250)]
        public string PermanentAddress { get; set; }

        [Required, MaxLength(250)]
        public string ResidentialAddress { get; set; }

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(150)]
        public string OtherContacts { get; set; }

        [Required, MaxLength(150)]
        public string Occupation { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal AnnualIncome { get; set; }
        public string Status { get; set; }

        public string CreatedById { get; set; }
        public bool deleteRequest;
    }
}

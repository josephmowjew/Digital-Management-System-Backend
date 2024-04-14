using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class License : Meta
    {

        public string LicenseNumber { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }
        public int LicenseApplicationId { get; set; }
        public LicenseApplication LicenseApplication { get; set; }
    }
}

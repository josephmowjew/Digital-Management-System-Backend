using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.Models
{
    public class YearOfOperation: Meta
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}

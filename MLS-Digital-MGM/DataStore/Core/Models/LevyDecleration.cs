using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;

namespace DataStore.Core.Models
{
    public class LevyDeclaration: Meta
    {
        public int Id { get; set; }
        public DateTime Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal LevyAmount { get; set; }
        public decimal Percentage { get; set; } // The percentage used for this calculation
        public int firmId { get; set; }
        public Firm Firm { get; set; }
    }
}

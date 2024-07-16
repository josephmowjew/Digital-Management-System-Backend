using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;

namespace DataStore.Core.Models
{
    public class LevyPercent : Meta
    {
        public int Id { get; set; }
        public double PercentageValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}

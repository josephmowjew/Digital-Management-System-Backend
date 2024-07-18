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
        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }
        public string OperationStatus { get; set; }
    }
}

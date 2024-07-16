using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;

namespace DataStore.Core.DTOs.LevyPercent
{
    public class CreateLevyPercentDTO
    {
        [Range(0, double.MaxValue, ErrorMessage = "Percentage value must be greater than or equal to 0")]
        public double PercentageValue { get; set; }
    }
}

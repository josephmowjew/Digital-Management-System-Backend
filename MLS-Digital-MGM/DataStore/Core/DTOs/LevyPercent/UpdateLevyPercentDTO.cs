using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;

namespace DataStore.Core.DTOs.LevyPercent
{
    public class UpdateLevyPercentDTO
    {
        public int Id { get; set; }
        [Required]
        public decimal PercentageValue { get; set; }
    }
}

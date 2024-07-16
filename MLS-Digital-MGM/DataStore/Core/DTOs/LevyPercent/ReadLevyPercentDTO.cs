using System;
using System.Collections.Generic;

namespace DataStore.Core.DTOs.LevyPercent
{
    public class ReadLevyPercentDTO
    {
        public ReadLevyPercentDTO()
        {
            // Initialize any collections if needed
        }

        public int Id { get; set; }
        public decimal Percentage { get; set; }
    }
}

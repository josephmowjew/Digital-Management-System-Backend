using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.YearOfOperation;

namespace DataStore.Core.DTOs.LevyPercent
{
    public class ReadLevyPercentDTO
    {
        public ReadLevyPercentDTO()
        {
            // Initialize any collections if needed
        }

        public int Id { get; set; }
        public decimal PercentageValue { get; set; }
        public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation { get; set; } 

    }
}

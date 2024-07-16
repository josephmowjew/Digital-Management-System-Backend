using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.Firms;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.LevyDeclaration
{
    public class ReadLevyDeclarationDTO
    {
        public ReadLevyDeclarationDTO()
        {
            // Initialize any collections if needed
        }

        public int Id { get; set; }
        public DateTime Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal LevyAmount { get; set; }
        public decimal Percentage { get; set; } // The percentage used for this calculation
        public int FirmId { get; set; }
        public ReadFirmDTO Firm { get; set;}
    }
}

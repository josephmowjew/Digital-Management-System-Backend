using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;

namespace DataStore.Core.DTOs.LevyDeclaration
{
    public class UpdateLevyDeclarationDTO
    {
        public int Id { get; set; }
        [Required]
        public DateTime Month { get; set; }
        [Required]
        public decimal Revenue { get; set; }
        [Required]
        public decimal LevyAmount { get; set; }
        [Required]
        public decimal Percentage { get; set; }
        public int FirmId { get; set; }
    }
}

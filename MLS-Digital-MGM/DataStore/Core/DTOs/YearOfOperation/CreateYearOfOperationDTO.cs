using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.YearOfOperation
{
    public class CreateYearOfOperationDTO
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}

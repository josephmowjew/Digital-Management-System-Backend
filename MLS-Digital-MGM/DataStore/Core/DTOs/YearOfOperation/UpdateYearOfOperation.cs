using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.YearOfOperation
{
    public class UpdateYearOfOperationDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
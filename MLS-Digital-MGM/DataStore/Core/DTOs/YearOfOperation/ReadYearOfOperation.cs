using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.YearOfOperation
{
    public class ReadYearOfOperationDTO
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
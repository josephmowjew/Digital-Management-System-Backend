using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.YearOfOperation
{
    public class ReadYearOfOperationDTO
    {
        public int Id { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string FormatedDate { get =>  $"{StartDate.Year} - {EndDate.Year}";
     }

       
    }
}
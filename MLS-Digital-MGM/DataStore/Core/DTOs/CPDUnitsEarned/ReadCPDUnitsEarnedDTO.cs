using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.CPDTraining;
using DataStore.Core.DTOs.Member;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.CPDUnitsEarned
{
    public class ReadCPDUnitsEarnedDTO
    {
        public int MemberId { get; set; }
        public ReadMemberDTO Member { get; set; }
        public int CPDTrainingId { get; set; }
        public ReadCPDTrainingDTO CPDTraining { get; set; }
        public int UnitsEarned { get; set; }
        public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation { get; set; }
    }
}

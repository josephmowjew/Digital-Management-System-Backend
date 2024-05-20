using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.LicenseApplication;
using DataStore.Core.DTOs.Member;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.License
{
    public class ReadLicenseDTO
    {
        public ReadLicenseDTO()
        {
            // Initialize any collections if needed
        }

        public string LicenseNumber { get; set; }
        public int MemberId { get; set; }
        public ReadMemberDTO Member { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation { get; set; }
        public int LicenseApplicationId { get; set; }
        public ReadLicenseApplicationDTO LicenseApplication { get; set; }
    }
}

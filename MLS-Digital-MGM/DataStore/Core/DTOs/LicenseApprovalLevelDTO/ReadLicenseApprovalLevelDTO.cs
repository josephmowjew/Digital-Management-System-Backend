using System;
using System.Collections.Generic;


namespace DataStore.Core.DTOs.LicenseApprovalLevelDTO
{
    public class ReadLicenseApprovalLevelDTO
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int DepartmentId { get; set; }
        public DataStore.Core.Models.Department Department { get; set; }
    }
}

using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.Department;


namespace DataStore.Core.DTOs.LicenseApprovalLevelDTO
{
    public class ReadLicenseApprovalLevelDTO
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int DepartmentId { get; set; }
        public ReadDepartmentDTO Department { get; set; }
    }
}

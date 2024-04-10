using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DataStore.Core.DTOs.LicenseApprovalLevelDTO
{
    public class UpdateLicenseApprovalLevelDTO 
    {
        public int Id { get; set; }
        [Required]
        public int Level { get; set; }
        [Required]
        public int DepartmentId { get; set; }
    }
}

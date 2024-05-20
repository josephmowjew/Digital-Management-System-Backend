using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.LicenseApprovalHistory
{
    public class CreateLicenseApprovalHistoryDTO
    {
        public int LicenseApplicationId { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ApprovalLevelId { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string Status { get; set; }
        [Required]
        public string ChangedById { get; set; }
       
    }
}

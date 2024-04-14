using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.Models
{
    public class LicenseApplicationApproval : Meta
    {

        [Required]
        public int LicenseApplicationID { get; set; }
        public LicenseApplication LicenseApplication { get; set; }

        [Required]
        public int LicenseApprovalLevelID { get; set; }
        public LicenseApprovalLevel LicenseApprovalLevel { get; set; }

        public bool Approved { get; set; }
        [StringLength(maximumLength:250)]
        public string Reason_for_Rejection { get; set; }

        public string? CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}

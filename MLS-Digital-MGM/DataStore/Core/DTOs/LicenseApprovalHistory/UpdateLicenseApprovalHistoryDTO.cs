using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.LicenseApprovalHistory
{
    public class UpdateLicenseApprovalHistoryDTO
    {
        public int Id { get; set; }
        public int LicenseApplicationId { get; set; }
        [Required]
        public DateTime ChangeDate { get; set; }
        [Required]
        public int ApprovalLevelId { get; set; }
        [Required]
        [StringLength(100)]
        public string Status { get; set; }
        [Required]
        public string ChangedById { get; set; }
    }
}

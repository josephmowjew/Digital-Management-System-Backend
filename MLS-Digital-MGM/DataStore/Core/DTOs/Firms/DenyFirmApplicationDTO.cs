using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Firms
{
    public class DenyFirmApplicationDTO
    {
        [Required]
        public int FirmId { get; set; }

        [Required]
        [StringLength(maximumLength: 250)]
        public string Reason { get; set; }
    }
}

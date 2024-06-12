using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.PenaltyPayment;

public class DenyPenaltyPaymentDTO
{   [Required]
    public int PenaltyPaymentId { get; set; }
    [Required, MaxLength(255)]
    public string Reason { get; set; }
}
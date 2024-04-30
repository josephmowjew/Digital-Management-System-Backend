using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.ProBonoApplication;

public class DenyProBonoApplicationDTO
{   [Required]
     public int ProBonoApplicationId { get; set; }
    [Required, MaxLength(200)]
    public string Reason { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.NotaryPublic;

public class DenyNotaryPublicDTO
{   [Required]
     public int NotaryPublicId { get; set; }
    [Required, MaxLength(200)]
    public string Reason { get; set; }
}
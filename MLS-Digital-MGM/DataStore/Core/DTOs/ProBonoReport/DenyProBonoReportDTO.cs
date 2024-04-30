using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.ProBonoReport;

public class DenyProBonoReportDTO
{
    [Required]
    public int ProBonoReportId { get; set; }
    [Required, MaxLength(200)]
    public string Reason { get; set; }
}
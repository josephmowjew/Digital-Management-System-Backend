using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.MemberQualification
{
  public class CreateMemberQualificationDTO
  {
    [Required]
    [StringLength(maximumLength:250)]
    public string Name { get; set; }
    [Required]
    [StringLength(maximumLength: 250)]
    public string IssuingInstitution { get; set; }

    [Required]
    public DateTime DateObtained { get; set; }

    [Required]
    public int MemberId { get; set; }

    [Required]
    public int QualificationTypeId { get; set; }
    [FileSize(5242880)] // 5 MB
    [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt" })]
    public ICollection<IFormFile>? Attachments { get; set; }  
  }
}

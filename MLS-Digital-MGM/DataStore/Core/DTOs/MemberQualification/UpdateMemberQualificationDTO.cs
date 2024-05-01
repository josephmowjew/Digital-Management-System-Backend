using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.MemberQualification
{
  public class UpdateMemberQualificationDTO
  {
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    public int MemberId { get; set; }

    [Required]
    [StringLength(250)] 
    public string IssuingInstitution { get; set; }

    public DateTime DateObtained { get; set; }

    public int QualificationTypeId { get; set; }
    [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt" })]
    [FileSize(5242880)] // 5 MB
    public ICollection<IFormFile>? Attachments { get; set; }  
  }
}



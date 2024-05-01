using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStore.Core.Models
{
  public class MemberQualification: Meta
  {
    public MemberQualification()
    {
        this.Attachments = new List<Attachment>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public int MemberId { get; set; }
    [Required]
    [StringLength(maximumLength: 250)]
    public string IssuingInstitution { get; set; }
    public DateTime DateObtained { get; set; }
    public Member Member { get; set; }
    public int QualificationTypeId { get; set; }
    public QualificationType QualificationType { get; set; }
    public virtual List<Attachment> Attachments { get; set; }
  }
}


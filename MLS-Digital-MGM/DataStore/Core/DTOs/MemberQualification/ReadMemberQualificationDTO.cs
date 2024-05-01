using System;
using System.Collections.Generic;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.MemberQualification
{
  public class ReadMemberQualificationDTO
  {
    

    public int Id { get; set; }

    public string Name { get; set; }

    public int MemberId { get; set; }

    public string IssuingInstitution { get; set; }

    public DateOnly DateObtained { get; set; }

    public DataStore.Core.Models.Member Member { get; set; }

    public int QualificationTypeId { get; set; }

    public DataStore.Core.Models.QualificationType QualificationType { get; set; }

    public List<DataStore.Core.DTOs.Attachment.ReadAttachmentDTO> Attachments { get; set; }
  }
}

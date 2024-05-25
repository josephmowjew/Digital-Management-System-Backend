using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.Attachment;
using DataStore.Core.DTOs.CPDTraining;
using DataStore.Core.DTOs.Member;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.CPDTrainingRegistration
{
    public class ReadCPDTrainingRegistrationDTO
    {
        public ReadCPDTrainingRegistrationDTO()
        {
            Attachments = new List<ReadAttachmentDTO>();
        }

        public int Id { get; set; }
        public int MemberId { get; set; }
        public ReadMemberDTO Member { get; set; }
        public string RegistrationStatus { get; set; }
        public int CPDTrainingId { get; set; }
        public ReadCPDTrainingDTO CPDTraining { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public string? DeniedReason { get; set; }
        public virtual List<ReadAttachmentDTO> Attachments { get; set; }
    }
}

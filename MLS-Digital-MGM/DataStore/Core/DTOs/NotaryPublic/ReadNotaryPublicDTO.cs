using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.DTOs.Member;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.DTOs.Attachment;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.NotaryPublic
{
    public class ReadNotaryPublicDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public ReadMemberDTO Member { get; set; }
        public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation { get; set; }
        public string ApplicationStatus { get; set;} 
        public DateTime? ApprovedDate { get; set; }
        public DateTime? DenialDate { get; set; }
        public string DenialReason { get; set; }
        public List<ReadAttachmentDTO> Attachments { get; set; }
    }
}

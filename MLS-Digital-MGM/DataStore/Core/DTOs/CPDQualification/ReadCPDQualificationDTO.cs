using System;
using System.Collections.Generic;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.CPDQualification
{
    public class ReadCPDQualificationDTO
    {
        public int CPDTrainingId { get; set; }

        public int MemberId { get; set; }

        public DataStore.Core.Models.Member Member { get; set; }

        public DateTime DateGenerated { get; set; }

        public List<DataStore.Core.DTOs.Attachment.ReadAttachmentDTO> Attachments { get; set; }
    }
}

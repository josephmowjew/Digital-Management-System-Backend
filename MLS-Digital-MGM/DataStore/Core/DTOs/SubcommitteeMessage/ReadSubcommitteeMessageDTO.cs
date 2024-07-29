using DataStore.Core.DTOs.Attachment;
using DataStore.Core.DTOs.User;
using DataStore.Core.DTOs.Subcommittee;
using DataStore.Core.DTOs.SubcommitteeThread;
using System;
using System.Collections.Generic;

namespace DataStore.Core.DTOs.SubcommitteeMessage
{
    public class ReadSubcommitteeMessageDTO
    {
        public ReadSubcommitteeMessageDTO()
        {
            Attachments = new List<ReadAttachmentDTO>();
        }

        public int Id { get; set; }
        public int SubcommitteeID { get; set; }
        public ReadSubcommitteeDTO Subcommittee { get; set; }
        public string CreatedById { get; set; }
        public ReadUserDTO CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
        public List<ReadAttachmentDTO> Attachments { get; set; }
        public int? SubcommitteeThreadId { get; set; }
        public ReadSubcommitteeThreadDTO SubcommitteeThread { get; set; }
    }
}

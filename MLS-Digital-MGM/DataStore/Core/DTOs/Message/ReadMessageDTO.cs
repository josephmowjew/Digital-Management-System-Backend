using DataStore.Core.DTOs.Attachment;
using DataStore.Core.DTOs.Member;
using DataStore.Core.DTOs.Thread;
using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Core.DTOs.Message
{
    public class ReadMessageDTO
    {
        public ReadMessageDTO()
        {
            Attachments = new List<ReadAttachmentDTO>();
        }

        public int Id { get; set; }
        public int CommitteeID { get; set; }
        public int SenderID { get; set; }
        public ReadMemberDTO Sender { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
        public List<ReadAttachmentDTO> Attachments { get; set; }
        public int? ThreadID { get; set; }
        public ReadThreadDTO Thread { get; set; }
    }
}

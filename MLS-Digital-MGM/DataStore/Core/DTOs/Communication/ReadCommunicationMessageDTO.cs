using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.User;

namespace MLS_Digital_Management_System_Front_End.Core.DTOs.Communication
{
    public class ReadCommunicationMessageDTO
    {
        public ReadCommunicationMessageDTO(){
            Attachments = new List<DataStore.Core.DTOs.Attachment.ReadAttachmentDTO>();
        }
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string SentByUserId { get; set; }
        public ReadUserDTO SentByUser { get; set; }
        public DateTime SentDate { get; set; }
        public string Status { get; set; }
        public bool SentToAllUsers { get; set; }
        public List<string> TargetedRoles { get; set; }
        public List<string> TargetedDepartments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<DataStore.Core.DTOs.Attachment.ReadAttachmentDTO> Attachments { get; set; }
        
    }

    
}
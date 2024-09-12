
using System;
using DataStore.Core.DTOs.Attachment;
using DataStore.Core.DTOs.YearOfOperation;

namespace DataStore.Core.DTOs.Stamp
{
    public class ReadStampDTO
    {
        public ReadStampDTO()
        {
            // Initialize any collections if needed
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ReadAttachmentDTO> Attachments { get; set; }
        public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation{ get; set; }
    }
}

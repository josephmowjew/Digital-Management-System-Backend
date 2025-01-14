
using System;
using DataStore.Core.DTOs.Attachment;
using DataStore.Core.DTOs.YearOfOperation;

namespace DataStore.Core.DTOs.Signature
{
    public class ReadSignatureDTO
    {
        public ReadSignatureDTO()
        {
            // Initialize any collections if needed
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<ReadAttachmentDTO> Attachments { get; set; }
        public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation{ get; set; }
    }
}

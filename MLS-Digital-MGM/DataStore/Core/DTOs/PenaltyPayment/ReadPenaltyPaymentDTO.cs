using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.Attachment;
using DataStore.Core.DTOs.Penalty;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.PenaltyPayment
{
    public class ReadPenaltyPaymentDTO
    {
        public ReadPenaltyPaymentDTO()
        {
            Attachments = new List<ReadAttachmentDTO>();
        }

        public int PenaltyId { get; set; }
        public ReadPenaltyDTO Penalty { get; set; }
        public string PaymentStatus { get; set; }
        public string? Description { get; set; }
        public double Fee { get; set; }
        public List<ReadAttachmentDTO> Attachments { get; set; }
    }
}

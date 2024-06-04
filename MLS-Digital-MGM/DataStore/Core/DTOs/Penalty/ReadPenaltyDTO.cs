using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.Attachment;
using DataStore.Core.DTOs.PenaltyPayment;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.Penalty
{
    public class ReadPenaltyDTO
    {
        public ReadPenaltyDTO()
        {
            // Initialize any collections if needed
        }

        public int Id { get; set; }
        public int MemberId { get; set; }
        public DataStore.Core.DTOs.Member.ReadMemberDTO Member { get; set; }
        public int PenaltyTypeId { get; set; }
        public double Fee { get; set; }
        public DataStore.Core.DTOs.PenaltyType.ReadPenaltyTypeDTO PenaltyType { get; set; }
         public string PenaltyStatus { get; set; }
        public string Reason { get; set; }
        public string? ResolutionComment { get; set; }
        public string CreatedById { get; set; }
        public DataStore.Core.DTOs.User.ReadUserDTO CreatedBy { get; set; }
         public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation{ get; set; }
        public List<ReadAttachmentDTO> Attachments { get; set; }
        public List<ReadPenaltyPaymentDTO> PenaltyPayments { get; set; }
    }
}

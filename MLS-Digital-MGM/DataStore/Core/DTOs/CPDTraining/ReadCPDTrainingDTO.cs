using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.CPDTrainingRegistration;
using DataStore.Core.DTOs.CPDUnitsEarned;
using DataStore.Core.DTOs.InvoiceRequest;
using DataStore.Core.DTOs.User;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;
using DataStore.Helpers;

namespace DataStore.Core.DTOs.CPDTraining
{
    public class ReadCPDTrainingDTO
    {
        public ReadCPDTrainingDTO()
        {
            CPDUnitsEarned = new List<ReadCPDUnitsEarnedDTO>();
            CPDTrainingRegistration = new List<ReadCPDTrainingRegistrationDTO>();
            Attachments = new List<DataStore.Core.DTOs.Attachment.ReadAttachmentDTO>();
        }
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public double Duration { get; set; }
        public DateOnly DateToBeConducted { get; set; }
        public int ProposedUnits { get; set; }
        public int CPDUnitsAwarded { get; set; }
        public string AccreditingInstitution { get; set; }
        public string? AccreditingInstitutionRepresentativePosition { get; set; }
        public string CreatedById { get; set; }
        public double? MemberPhysicalAttendanceFee { get; set; } = 0;
        public double? MemberVirtualAttendanceFee { get; set; } = 0;
        public double? NonMemberPhysicalAttendanceFee { get; set; } = 0; 
        public double? NonMemberVirtualAttandanceFee { get; set; } = 0;
        public string? PhysicalVenue { get; set; }
        public ReadUserDTO CreatedBy { get; set; }
        public int YearOfOperationId { get; set; }
        
        public DateOnly RegistrationDueDate { get; set; }
        public bool IsFree { get; set; } = false;
        public ReadYearOfOperationDTO YearOfOperation { get; set; }
        public ICollection<ReadCPDUnitsEarnedDTO> CPDUnitsEarned { get; set; }
        public ICollection<ReadCPDTrainingRegistrationDTO> CPDTrainingRegistration { get; set; }
        public List<DataStore.Core.DTOs.Attachment.ReadAttachmentDTO> Attachments { get; set; }
        public int NumberOfPendingRegistrations { get; set; }
        public int? InvoiceRequestId { get; set; }
        public ReadInvoiceRequestDTO InvoiceRequest{ get; set; }
    }
}

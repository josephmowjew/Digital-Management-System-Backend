using DataStore.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.CPDTraining
{
    public class UpdateCPDTrainingDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public double Duration { get; set; }
        public double? MemberPhysicalAttendanceFee { get; set; } = 0;
        public double? MemberVirtualAttendanceFee { get; set; } = 0;
        public double? NonMemberPhysicalAttendanceFee { get; set; } = 0;
        public double? NonMemberVirtualAttandanceFee { get; set; } = 0;
        public double? SeniorLawyerPhysicalAttendanceFee { get; set; } = 0;
        public double? SeniorLawyerVirtualAttendanceFee { get; set; } = 0;
        public double? JuniorLawyerPhysicalAttendanceFee { get; set; } = 0;
        public double? JuniorLawyerVirtualAttendanceFee { get; set; } = 0;
        public string? PhysicalVenue { get; set; }
        public bool IsFree { get; set; } = false;
        public bool IsCategorizedForMembers { get; set; } = false;
        public DateTime DateToBeConducted { get; set; }
        [DateGreaterThanOrEqualToToday(ErrorMessage = "Registration due date must be today or a future date.")]
        public DateTime? RegistrationDueDate { get; set; }
        public int ProposedUnits { get; set; }
        public int CPDUnitsAwarded { get; set; }
        [StringLength(200)]
        public string? AccreditingInstitution { get; set; }
        public string? AccreditingInstitutionRepresentativePosition { get; set; }

        public List<IFormFile> Attachments { get; set; }

        public void SetDefaultValues()
        {
            MemberPhysicalAttendanceFee ??= 0;
            MemberVirtualAttendanceFee ??= 0;
            NonMemberPhysicalAttendanceFee ??= 0;
            NonMemberVirtualAttandanceFee ??= 0;
        }
    }
}

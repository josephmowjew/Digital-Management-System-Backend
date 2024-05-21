using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.CPDTraining;
using DataStore.Core.DTOs.Member;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.CPDTrainingRegistration
{
    public class ReadCPDTrainingRegistrationDTO
    {
        public ReadCPDTrainingRegistrationDTO()
        {

        }

        public int Id { get; set; }
        public int MemberId { get; set; }
        public ReadMemberDTO Member { get; set; }
        public string RegistrationStatus { get; set; }
        public int CPDTrainingId { get; set; }
        public ReadCPDTrainingDTO CPDTraining { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
    }
}

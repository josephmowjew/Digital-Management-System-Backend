using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Member
{
    public class ReadMemberDTO
    {
        public string Id { get; set; }

        public string UserId { get; set; }

       
        public string PostalAddress { get; set; }

  
        public string PermanentAddress { get; set; }

    
        public string ResidentialAddress { get; set; }

        [Required]
        public DateOnly DateOfAdmissionToPractice { get; set; }

        public List<DataStore.Core.DTOs.QualificationType.ReadQualificationTypeDTO> QualificationTypes { get; set; }

        public List<DataStore.Core.DTOs.ProBono.ReadProBonoDTO> ProBonos { get; set; }
    }
}

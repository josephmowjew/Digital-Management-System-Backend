using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.DTOs.Firms;
using DataStore.Core.DTOs.License;
using DataStore.Core.DTOs.User;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.Member
{
    public class ReadMemberDTO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

       
        public string PostalAddress { get; set; }

  
        public string PermanentAddress { get; set; }

    
        public string ResidentialAddress { get; set; }

        [Required]
        public DateOnly DateOfAdmissionToPractice { get; set; }

        public List<DataStore.Core.DTOs.QualificationType.ReadQualificationTypeDTO> QualificationTypes { get; set; }

        public List<DataStore.Core.DTOs.ProBono.ReadProBonoDTO> ProBonos { get; set; }

        public ReadUserDTO User { get; set; }

        public int FirmId { get; set; }
        public ReadFirmDTO Firm { get; set; }
        public List<ReadLicenseDTO> Licenses { get; set; }

        public ReadLicenseDTO CurrentLicense { get; set; }
        public string? CustomerId {get; set;}
        public QBCustomer Customer {get; set;}  
    }
}

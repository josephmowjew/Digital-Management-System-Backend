using DataStore.Core.DTOs.Department;
using System;
using System.Collections.Generic;  
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.User {

  public class ReadUserDTO {

    
    public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OtherName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime IdentityExpiryDate { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }

        public int IdentityTypeId { get; set; }

        public int TitleId { get; set; }
        public int CountryId { get; set; }

        public string RoleName { get; set; }
        public string Email { get; set; }


  }

}




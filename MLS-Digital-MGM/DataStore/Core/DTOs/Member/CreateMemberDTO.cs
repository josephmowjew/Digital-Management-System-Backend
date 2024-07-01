using System;
using System.Collections.Generic;  
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Member
{
  public class CreateMemberDTO
  {

    public string UserId { get; set; }
    [Required, MaxLength(250)]
    public string PostalAddress { get; set; }

    [Required, MaxLength(250)]
    public string PermanentAddress { get; set; }

    [Required, MaxLength(250)] 
    public string ResidentialAddress { get; set; }

    [Required]
    public DateTime DateOfAdmissionToPractice { get; set; }
    public string? CustomerId {get; set;}

  }
}



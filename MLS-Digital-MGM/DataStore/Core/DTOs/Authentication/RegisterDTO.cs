using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.Authentication
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(maximumLength: 100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    
        [StringLength(maximumLength: 100)]
        [Display(Name = "Other Name")]
        public string? OtherName { get; set; }
        [Required]
        [StringLength(maximumLength: 15)]
        public string Gender { get; set; }
        [StringLength(maximumLength: 15, MinimumLength = 10)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        [Display(Name = "Identity Number")]
        public string IdentityNumber { get; set; }
        [Display(Name = "Identity Expiry Date")]
        public DateTime IdentityExpiryDate { get; set; }
        [Required]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }

        [Display(Name = "Identity Type")]
        [Required]
        public int IdentityTypeId { get; set; }

        [Display(Name = "Title")]
        public int TitleId { get; set; }
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "Role")]
        [Required]
        public string RoleName { get; set; }


        [Required]
        [EmailAddress]
        [StringLength(100, MinimumLength = 5)]
        public string Email { get; set; }
        public string Password { get; set; } = "P@$$w0rd";

       
    }
}

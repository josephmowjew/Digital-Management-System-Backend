using DataStore.Core.Models.Interfaces;
using DataStore.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class ApplicationUser: IdentityUser, IEntity
    {
        public ApplicationUser()
        {
            Penalties = new List<Penalty>();
            Committees = new List<Committee>();
        }

        [Required]
        [StringLength(maximumLength: 100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string LastName { get; set; }
       
        [StringLength(maximumLength: 100)]
        public string? OtherName { get; set; }
        [Required]
        [StringLength(maximumLength: 15)]
        public string Gender { get; set; }
        [StringLength(maximumLength: 15, MinimumLength = 10)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string IdentityNumber { get; set; }
        public DateTime IdentityExpiryDate { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        //add association to Department
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        //add association to IdentityType
        public int IdentityTypeId { get; set; }
        public IdentityType IdentityType { get; set; }

        //add association to Title
        public int TitleId { get; set; }
        public Title Title { get; set; }

        //add association to Country
        public int CountryId { get; set; }
        public Country Country { get; set; }

        //Meta properties
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? CreatedById { get; set; }
        public string Status { get; set; } = Lambda.Active;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }
        public DateTime LastLogin { get; set; }
        public int? Pin { get; set; }
        public List<Penalty> Penalties { get; set; }

        public List<Committee> Committees { get; set; }

       
        



    }
}

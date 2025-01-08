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
    public class ApplicationUserChangeRequest: IEntity
    {
        public int Id { get ; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
       
        [Required]
        public string Email { get; set; }

        public string? RejectionReason { get; set; }

        //Meta properties
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? CreatedById { get; set; }
        public string Status { get; set; } = Lambda.Pending;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }
    }
}
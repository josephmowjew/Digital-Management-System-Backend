using DataStore.Core.Models.Interfaces;
using DataStore.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class Role: IdentityRole, IEntity
    {
        //Meta properties
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedById { get; set; }
        public string Status { get; set; } = Lambda.Active;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public DateTime? DeletedDate { get; set; }
    }
}

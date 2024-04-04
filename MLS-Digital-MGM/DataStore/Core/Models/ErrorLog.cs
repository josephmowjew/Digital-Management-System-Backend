using DataStore.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class ErrorLog: Meta
    {

        [Required]
        public string UserFriendlyMessage { get; set; }
        [Required]
        public string DetailedMessage { get; set; }
        [Required]
        public DateTime DateOccurred { get; set; }

        public string? CreatedById { get; set; }
        public virtual ApplicationUser? CreatedBy { get; set; }
    }
}

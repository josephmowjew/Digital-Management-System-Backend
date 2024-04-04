using DataStore.Core.Models.Interfaces;
using DataStore.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class Meta : IEntity
    {
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Display(Name = "Created By Id")]

        //public string? CreatedById { get; set; }
        //[Display(Name = "Created By")]
        //public virtual ApplicationUser CreatedBy { get; set; }
        public string Status { get; set; } = Lambda.Active;
        [Display(Name = "Update Date")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [Display(Name = "Deleted Date")]
        public DateTime? DeletedDate { get; set; }
        public int Id { get ; set; }
    }
}

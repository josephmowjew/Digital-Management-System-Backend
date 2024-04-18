using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models.Interfaces
{
    public interface IEntity
    {
       
        public DateTime CreatedDate { get; set; }
        //public string? CreatedById { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
      
    }
}

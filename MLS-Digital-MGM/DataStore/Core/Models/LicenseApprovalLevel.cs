using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class LicenseApprovalLevel: Meta
    {
       //create a property for id and name
        public int Id { get; set; }
        [Required]
        public int Level {get; set;}
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
  
    }
}

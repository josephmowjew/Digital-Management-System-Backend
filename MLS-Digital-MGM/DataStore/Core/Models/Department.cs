using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class Department: Meta<int>
    {
        public Department()
        {
            Users = new List<ApplicationUser>();
        }
       
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Name{ get; set; }

        //add association to ApplicationUser
        public ICollection<ApplicationUser> Users { get; set; }

    }
}

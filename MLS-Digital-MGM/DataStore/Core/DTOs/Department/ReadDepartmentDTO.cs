using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs.Department
{
    public class ReadDepartmentDTO
    {
        public ReadDepartmentDTO()
        {
            Users = new List<ApplicationUser>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:150)]
        public string Name { get; set; } = string.Empty;
        //add association to ApplicationUser
        public ICollection<ApplicationUser> Users { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs.Department
{
    public class CreateDepartmentDTO
    {
        [Required]
        [StringLength(maximumLength:150)]
        public string Name { get; set; }
    }
}

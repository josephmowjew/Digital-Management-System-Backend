using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs.Role
{
    public class CreateRoleDTO
    {
        [Required]
        [StringLength(maximumLength:100)]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs.Role
{
    public class UpdateRoleDTO
    {
        public string Id { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
    }
}

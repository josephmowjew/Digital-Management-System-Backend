using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class Title: Meta
    {
        [Required]
        [StringLength(maximumLength:50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}

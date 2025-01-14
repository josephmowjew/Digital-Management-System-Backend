using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class InstitutionType : Meta
    {
        [Required]
        [StringLength(maximumLength: 150, MinimumLength = 3)]
        public string Name { get; set; }

        // Navigation property for related Firms    
        public virtual ICollection<Firm> Firms { get; set; } = new List<Firm>();
    }
}

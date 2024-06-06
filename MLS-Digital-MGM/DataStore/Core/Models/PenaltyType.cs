using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class PenaltyType: Meta
    {
        public PenaltyType()
        {
            this.Penalties = new List<Penalty>();
        }
        [StringLength(maximumLength:100)]
        public string Name { get; set; }
        [StringLength(maximumLength:250)]
        public string Description { get; set; }
        public List<Penalty> Penalties { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class Country: Meta<int>
    {
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength:10,MinimumLength =2)]
        public string ShortCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class AttachmentType: Meta
    {
        [Required]
        [StringLength(maximumLength:200)]
        public string Name { get; set; }
    }
}

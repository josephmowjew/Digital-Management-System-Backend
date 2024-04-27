using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class QualificationType : Meta
    {
        public QualificationType()
        {
            this.Members = new List<Member>();
            //this.Attachments = new List<Attachment>();
        }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
        public virtual List<Member> Members { get; set; }
       
    }
}

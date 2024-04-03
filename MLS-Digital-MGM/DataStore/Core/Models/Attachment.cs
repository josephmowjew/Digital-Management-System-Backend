using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class Attachment: Meta<int>
    {
        public Attachment()
        {
            Users = new List<ApplicationUser>();
        }
        
        [Required]
        [StringLength(maximumLength:200)]
        public string FileName { get; set; }
        [Required]
        [StringLength(maximumLength:250)]
        public string FilePath { get; set; }

        //add association to AttachmentType

        public int AttachmentTypeId { get; set; }
        public AttachmentType AttachmentType { get; set; }

        //add many to many association to User

        public ICollection<ApplicationUser> Users { get; set; } 


    }
}

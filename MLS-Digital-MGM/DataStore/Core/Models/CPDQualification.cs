using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.Models
{
    public class CPDQualification : Meta
    {
        public CPDQualification()
        {
            this.Attachments = [];
        }
        
        [Required]
        public int CPDTrainingId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public DateTime DateGenerated { get; set; }

        public virtual List<Attachment> Attachments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Thread
{
    public class CreateThreadDTO
    {
        public int CommitteeId { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string Subject { get; set; }

        

        
    }
}

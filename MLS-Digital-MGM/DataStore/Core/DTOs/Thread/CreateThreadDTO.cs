using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Thread
{
    public class CreateThreadDTO
    {
        public int CommitteeID { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Subject { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.SubcommitteeThread
{
    public class CreateSubcommitteeThreadDTO
    {
        public int SubcommitteeId { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string Subject { get; set; }
    }
}

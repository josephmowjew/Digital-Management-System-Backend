using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Committee
{
    public class UpdateCommitteeDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string CommitteeName { get; set; }
        [StringLength(maximumLength: 500)]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int? ChairpersonID { get; set; }
        public int YearOfOperationId { get; set; }
        
    }
}

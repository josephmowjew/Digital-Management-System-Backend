using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.QualificationType
{
    public class CreateQualificationTypeDTO
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Name { get; set; }
    }
}

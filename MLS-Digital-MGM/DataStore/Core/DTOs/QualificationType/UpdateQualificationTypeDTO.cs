using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.QualificationType
{
    public class UpdateQualificationTypeDTO
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}

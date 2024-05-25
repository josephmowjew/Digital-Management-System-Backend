using DataStore.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.CPDTraining
{
    public class CreateCPDTrainingDTO
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Title { get; set; }

        [StringLength(maximumLength: 250)]
        public string Description { get; set; }

        [Required]
        public double Duration { get; set; }

        [Required]
        public DateTime DateToBeConducted { get; set; }

        [Required]
        public int ProposedUnits { get; set; }

        public double? TrainingFee { get; set; }

        [Required]
        public int CPDUnitsAwarded { get; set; }

        [StringLength(maximumLength: 200)]
        public string AccreditingInstitution { get; set; }

        [AllowedFileTypes(new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt",".jpg",".png",".jpeg" })]
        [FileSize(5242880)] // 5 MB
        public List<IFormFile?> Attachments { get; set; }  
    }
}

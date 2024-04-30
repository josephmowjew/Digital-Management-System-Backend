using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DataStore.Core.Models;
using Microsoft.AspNetCore.Http;

namespace DataStore.Core.DTOs.ProBonoApplication
{
    public class UpdateProBonoApplicationDTO
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string NatureOfDispute { get; set; }

        [Required]
        public string CaseDetails { get; set; }

        public int ProbonoClientId { get; set; }

        
        public string SummaryOfDispute { get; set; }
        public int YearOfOperationId { get; set; }

        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<IFormFile>? Attachments { get; set; } 
    }
}

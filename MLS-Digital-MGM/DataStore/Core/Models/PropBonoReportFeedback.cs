using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class PropBonoReportFeedback: Meta
    {
        public int ProBonoReportId { get; set; }
        public ProBonoReport ProBonoReport { get; set; }
        [Required]
        [StringLength(maximumLength:250)]
        public string Feedback { get; set; }
        public string? FeedBackById { get; set; }
        public ApplicationUser FeedBackBy { get; set; }
    }
}

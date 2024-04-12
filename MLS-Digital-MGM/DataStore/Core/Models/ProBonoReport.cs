using DataStore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class ProBonoReport: Meta
    {
        public int ProBonoId { get; set; }
        public ProBono ProBono { get; set; }
        public double ProBonoProposedHours { get; set; }
        public double ProBonoHours {  get; set; }
        public string ReportStatus { get; set; } = Lambda.Pending;
        public string? ApprovedById { get; set; }
        public ApplicationUser ApprovedBy { get; set; }
    }
}

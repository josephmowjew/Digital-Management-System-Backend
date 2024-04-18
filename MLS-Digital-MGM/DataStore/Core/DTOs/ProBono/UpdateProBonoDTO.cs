using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs.ProBono
{
    public class UpdateProBonoDTO
    {
        public int Id { get; set; }

        

        [Required, MaxLength(200)]
        public string NatureOfDispute { get; set; }

        [Required]
        public string CaseDetails { get; set; }

        public string SummaryOfDispute { get; set; }

        public int ProbonoClientId { get; set; }
        public int YearOfOperationId { get; set; }
    }
}

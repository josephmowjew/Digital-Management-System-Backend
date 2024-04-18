using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.DTOs.ProBonoClient;

namespace DataStore.Core.DTOs.ProBono
{
    public class ReadProBonoDTO
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FileNumber { get; set; }

        [Required, MaxLength(200)]
        public string NatureOfDispute { get; set; }

        [Required]
        public string CaseDetails { get; set; }

        public string SummaryOfDispute { get; set; }

        public int ProbonoClientId { get; set; }

        public ReadProBonoClientDTO ProbonoClient { get; set; }

        public int ProBonoApplicationId { get; set; }
        public DataStore.Core.Models.ProBonoApplication ProBonoApplication { get; set; }
    }
}

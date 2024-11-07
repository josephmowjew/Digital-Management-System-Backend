using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.DTOs.Member;
using DataStore.Core.DTOs.ProBonoClient;
using DataStore.Core.DTOs.ProBonoReport;

namespace DataStore.Core.DTOs.ProBono
{
    public class ReadProBonoDTO
    {
        public ReadProBonoDTO()
        {
            ProBonoReports = new List<ReadProBonoReportDTO>();
        }
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
        public ICollection<ReadMemberDTO> Members { get; set; }
        public int ProBonoApplicationId { get; set; }
        public DataStore.Core.Models.ProBonoApplication ProBonoApplication { get; set; }
        public int YearOfOperationId { get; set; }
        public DataStore.Core.DTOs.YearOfOperation.ReadYearOfOperationDTO YearOfOperation { get; set; }
        public ICollection<ReadProBonoReportDTO> ProBonoReports { get; set; }

        public double ProBonoHoursAccoumulated { get; set; }
    }
}

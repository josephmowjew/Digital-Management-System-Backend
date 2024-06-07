using DataStore.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Thread
{
    public class ReadThreadDTO
    {
        public int CommitteeID { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByMemberName { get; set; }
    }
}

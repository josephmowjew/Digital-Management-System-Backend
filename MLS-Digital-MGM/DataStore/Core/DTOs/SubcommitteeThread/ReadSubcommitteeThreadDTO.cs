using DataStore.Core.DTOs.User;
using DataStore.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.SubcommitteeThread
{
    public class ReadSubcommitteeThreadDTO
    {
        public int Id { get; set; }
        public int SubcommitteeId { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public ReadUserDTO CreatedBy { get; set; }
    }
}

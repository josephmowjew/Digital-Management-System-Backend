using DataStore.Core.DTOs.User;
using DataStore.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Thread
{
    public class ReadThreadDTO
    {
        public int Id { get; set; }   
        public int CommitteeId { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public ReadUserDTO CreatedBy { get; set; }
    }
}

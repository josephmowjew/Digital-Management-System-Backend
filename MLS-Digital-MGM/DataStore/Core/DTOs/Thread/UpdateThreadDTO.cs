using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Thread
{
    public class UpdateThreadDTO
    {
        public int Id { get; set; }
        public int CommitteeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Subject { get; set; }
       
       
    }
}

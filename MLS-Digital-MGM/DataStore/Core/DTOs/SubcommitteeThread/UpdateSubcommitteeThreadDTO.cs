using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.SubcommitteeThread
{
    public class UpdateSubcommitteeThreadDTO
    {
        public int Id { get; set; }
        public int SubcommitteeId { get; set; }
        [Required]
        [StringLength(100)]
        public string Subject { get; set; }
    }
}

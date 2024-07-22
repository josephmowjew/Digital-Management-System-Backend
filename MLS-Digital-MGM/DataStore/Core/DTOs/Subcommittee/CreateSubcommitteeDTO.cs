using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Subcommittee
{
    public class CreateSubcommitteeDTO
    {
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string SubcommitteeName { get; set; }

        [StringLength(maximumLength: 500)]
        public string Description { get; set; }

        public int? ChairpersonId { get; set; }

        [Required]
        public int CommitteeId { get; set; }

        public string CreatedById { get; set; }
    }
}

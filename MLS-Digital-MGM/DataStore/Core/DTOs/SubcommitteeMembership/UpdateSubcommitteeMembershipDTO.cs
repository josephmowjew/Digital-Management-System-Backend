using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.SubcommitteeMembership
{
    public class UpdateSubcommitteeMembershipDTO
    {
        public int Id { get; set; }

        public int SubcommitteeID { get; set; }

        public string MemberShipId { get; set; }

        [Required]
        public DateTime JoinedDate { get; set; }

        public string? MemberShipStatus { get; set; }

        [StringLength(maximumLength: 150)]
        public string Role { get; set; }
    }
}

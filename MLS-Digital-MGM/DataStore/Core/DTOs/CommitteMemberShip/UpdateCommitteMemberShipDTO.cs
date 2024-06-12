using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.CommitteeMemberShip
{
    public class UpdateCommitteeMemberShipDTO
    {
        public int Id { get; set; }

        public int CommitteeID { get; set; }

        public string  MemberShipId { get; set; }

        [Required]
        public DateTime JoinedDate { get; set; }
    }
}

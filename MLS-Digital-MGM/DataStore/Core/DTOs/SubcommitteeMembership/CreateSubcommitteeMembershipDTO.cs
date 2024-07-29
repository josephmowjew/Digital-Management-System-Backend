using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.SubcommitteeMembership
{
    public class CreateSubcommitteeMembershipDTO
    {
        public int SubcommitteeID { get; set; }
        
        public string MembershipId { get; set; }

        public DateTime? JoinedDate { get; set; }
        
        public string Role { get; set; }
    }
}

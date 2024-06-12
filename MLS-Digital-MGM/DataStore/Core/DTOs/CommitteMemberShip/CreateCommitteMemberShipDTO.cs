using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.CommitteeMemberShip
{
    public class CreateCommitteeMemberShipDTO
    {
        public int CommitteeID { get; set; }
        
        public string  MemberShipId { get; set; }

       
        public DateTime? JoinedDate { get; set; }
        public string Role { get; set; }

     
    }
}

using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.CommitteMember
{
    public class CreateCommitteMemberShipDTO
    {
        public int CommitteeID { get; set; }
        
        public int MemberID { get; set; }

        [Required]
        public DateTime JoinedDate { get; set; }

     
    }
}

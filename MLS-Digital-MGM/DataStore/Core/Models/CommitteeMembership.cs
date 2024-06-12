using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.Models;

public class CommitteeMembership : Meta
{
    

    public int CommitteeID { get; set; }
    public Committee Committee { get; set; }

    public string MemberShipId { get; set; }
    public ApplicationUser MemberShip { get; set; }

    public DateTime JoinedDate { get; set; }
    [StringLength(maximumLength:150)]
    public string Role { get; set; }
    
}

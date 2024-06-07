namespace DataStore.Core.Models;

public class CommitteeMembership : Meta
{
    

    public int CommitteeID { get; set; }
    public Committee Committee { get; set; }

    public int MemberID { get; set; }
    public Member Member { get; set; }

    public DateTime JoinedDate { get; set; }
    
}

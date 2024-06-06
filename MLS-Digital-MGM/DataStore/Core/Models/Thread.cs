namespace DataStore.Core.Models;

public class Thread: Meta
{

    public int CommitteeID { get; set; }
    public Committee Committee { get; set; }

    public string Subject { get; set; }
    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }
    public Member CreatedByMember { get; set; }

    // Navigation Properties
    public ICollection<Message> Messages { get; set; }
}

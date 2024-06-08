namespace DataStore.Core.Models;

public class Thread: Meta
{

    public int CommitteeId { get; set; }
    public Committee Committee { get; set; }

    public string Subject { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public string CreatedById { get; set; } 
    public ApplicationUser CreatedBy { get; set; }

    // Navigation Properties
    public ICollection<Message> Messages { get; set; }
}

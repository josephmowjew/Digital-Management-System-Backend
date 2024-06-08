namespace DataStore.Core.Models;

public class Message: Meta
{
    

    public int CommitteeID { get; set; }
    public Committee Committee { get; set; }

    public string CreatedById { get; set; }
    public ApplicationUser CreatedBy { get; set; } 

    public DateTime Timestamp { get; set; }
    public string Content { get; set; }
    public List<Attachment> Attachments { get; set; }

    public int? ThreadId { get; set; }
    public Thread Thread { get; set; }
}

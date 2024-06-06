namespace DataStore.Core.Models;

public class Message: Meta
{
    

    public int CommitteeID { get; set; }
    public Committee Committee { get; set; }

    public int SenderID { get; set; }
    public Member Sender { get; set; }

    public DateTime Timestamp { get; set; }
    public string Content { get; set; }
    public List<Attachment> Attachments { get; set; }

    public int? ThreadID { get; set; }
    public Thread Thread { get; set; }
}

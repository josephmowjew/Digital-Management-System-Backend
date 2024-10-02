namespace DataStore.Core.Models;

public class EmailQueue : Meta
{
    public int Id { get; set; }
    public string RecipientEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime ScheduledDate { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentDate { get; set; }
    public string EmailType { get; set; } // e.g., "Welcome", "MissingFields", etc.

    
}

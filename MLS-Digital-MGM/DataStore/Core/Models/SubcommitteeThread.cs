namespace DataStore.Core.Models;

public class SubcommitteeThread : Meta
{
    public int SubcommitteeId { get; set; }
    public Subcommittee Subcommittee { get; set; }

    public string Subject { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public string CreatedById { get; set; }
    public ApplicationUser CreatedBy { get; set; }

    // Navigation Properties
    public ICollection<SubcommitteeMessage> SubcommitteeMessages { get; set; }
}

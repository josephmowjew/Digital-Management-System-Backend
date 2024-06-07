namespace DataStore.Core.Models;
public class Committee : Meta
{
  
    public string CommitteeName { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
  
    public int? ChairpersonID { get; set; }
    public Member Chairperson { get; set; }
    public int YearOfOperationId { get; set; }

    // Navigation Properties
    public YearOfOperation YearOfOperation { get; set; }
    public ICollection<CommitteeMembership> CommitteeMemberships { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<Thread> Threads { get; set; }
    public string CreatedById { get; set; }
    public ApplicationUser CreatedBy { get; set; } 
}
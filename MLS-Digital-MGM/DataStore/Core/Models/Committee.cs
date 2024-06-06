namespace DataStore.Core.Models;
public class Committee : Meta
{
  
    public string CommitteeName { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    public string MeetingSchedule { get; set; }

    public int? ChairpersonID { get; set; }
    public Member Chairperson { get; set; }

    // Navigation Properties
    public ICollection<CommitteeMembership> CommitteeMemberships { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<Thread> Threads { get; set; }
}
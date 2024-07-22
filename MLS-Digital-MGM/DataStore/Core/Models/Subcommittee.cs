using System;
using System.Collections.Generic;

namespace DataStore.Core.Models;

public class Subcommittee : Meta
{
    public string SubcommitteeName { get; set; }
    public string Description { get; set; }

    public int? ChairpersonId { get; set; }
    public Member Chairperson { get; set; }
    public int CommitteeId { get; set; }

    // Navigation Properties
    public Committee ParentCommittee { get; set; }
    public ICollection<SubcommitteeMembership> SubcommitteeMemberships { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<Thread> Threads { get; set; }
    public string CreatedById { get; set; }
    public ApplicationUser CreatedBy { get; set; }
}

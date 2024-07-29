using System;
using System.Collections.Generic;

namespace DataStore.Core.Models
{
    public class SubcommitteeMessage : Meta
    {
        public int SubcommitteeID { get; set; }
        public Subcommittee Subcommittee { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public string Content { get; set; }
        public List<Attachment> Attachments { get; set; }
        public int? SubcommitteeThreadId { get; set; }
        public SubcommitteeThread SubcommitteeThread { get; set; }
    }
}

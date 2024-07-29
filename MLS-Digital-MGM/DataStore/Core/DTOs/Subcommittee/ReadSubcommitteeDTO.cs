using DataStore.Core.DTOs.Committee;
using DataStore.Core.DTOs.Member;
using DataStore.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataStore.Core.DTOs.Subcommittee
{
    public class ReadSubcommitteeDTO
    {
        public int Id { get; set; }
        public string SubcommitteeName { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int? ChairpersonId { get; set; }
        public ReadMemberDTO Chairperson { get; set; }
        public int CommitteeId { get; set; }
        public ReadCommitteeDTO ParentCommittee { get; set; }

        public List<Models.SubcommitteeMembership> SubcommitteeMemberships { get; set; }
        public List<CommitteeMembership> CommitteeMemberships { get; set; }
    }
}

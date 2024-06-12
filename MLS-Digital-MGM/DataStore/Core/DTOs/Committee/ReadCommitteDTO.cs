using DataStore.Core.DTOs.Member;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.Committee
{
    public class ReadCommitteeDTO
    {
        public int Id { get; set; }
        public string CommitteeName { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int? ChairpersonID { get; set; }
        public ReadMemberDTO Chairperson { get; set; }
        public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation { get; set; }

        public List<CommitteeMembership> CommitteeMemberships { get; set; }

    }

    
}

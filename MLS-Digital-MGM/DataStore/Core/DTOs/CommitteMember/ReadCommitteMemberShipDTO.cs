using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.DTOs.YearOfOperation;

namespace DataStore.Core.DTOs.CommitteMember
{
    public class ReadCommitteMemberShipDTO
    {
        public int Id { get; set; }

        public int CommitteeID { get; set; }

        public int MemberID { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; }
        public int YearOfOperationId { get; set; }
        public ReadYearOfOperationDTO YearOfOperation { get; set; }
    }
}

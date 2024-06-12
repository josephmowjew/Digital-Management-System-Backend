using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.DTOs.YearOfOperation;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.CommitteeMemberShip
{
    public class ReadCommitteeMemberShipDTO
    {
        public int Id { get; set; }

        public int CommitteeID { get; set; }

        public string  MemberShipId { get; set; }

        public ApplicationUser MemberShip { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; }


        public  string Role { get; set; }
    }
}

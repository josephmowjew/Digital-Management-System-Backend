using System;
using System.ComponentModel.DataAnnotations;
using DataStore.Core.DTOs.User;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.SubcommitteeMembership
{
    public class ReadSubcommitteeMembershipDTO
    {
        public int Id { get; set; }

        public int SubcommitteeID { get; set; }

        public string MemberShipId { get; set; }

        public ReadUserDTO MemberShip { get; set; }

        public string? MemberShipStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoinedDate { get; set; }

        [StringLength(maximumLength: 150)]
        public string Role { get; set; }
    }
}

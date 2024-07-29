using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DataStore.Core.Models;

public class SubcommitteeMembership : Meta
{
    public int SubcommitteeID { get; set; }
    public Subcommittee Subcommittee { get; set; }

    public string MemberShipId { get; set; }
    public ApplicationUser MemberShip { get; set; }

    public string? MemberShipStatus { get; set; }

    public DateTime JoinedDate { get; set; }
    [StringLength(maximumLength: 150)]
    public string Role { get; set; }
}

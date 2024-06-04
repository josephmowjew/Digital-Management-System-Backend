using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataStore.Helpers;

namespace DataStore.Core.Models
{
    public class PenaltyPayment : Meta
    {
        public int PenaltyId { get; set; }
        public Penalty Penalty { get; set; }
        public string PaymentStatus { get; set; } = Lambda.Pending;
        public string? Description { get; set; }
        public double Fee { get; set; }
        public List<Attachment> Attachments { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Helpers;

namespace DataStore.Core.Models
{
    public class Penalty: Meta
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int PenaltyTypeId {  get; set; }
        public PenaltyType PenaltyType { get; set; }
        public double Fee { get; set; }
        public string Reason { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation{ get; set; }
        public List<Attachment> Attachments { get; set; }
        [StringLength(maximumLength:100)]
        public string PenaltyStatus { get; set; } = Lambda.Issued;
        [StringLength(maximumLength:250)]
        public string? ResolutionComment { get; set; }
        public List<PenaltyPayment> PenaltyPayments { get; set; }
        
    }
}

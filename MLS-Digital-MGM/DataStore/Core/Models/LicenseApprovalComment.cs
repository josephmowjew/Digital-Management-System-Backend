using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class LicenseApprovalComment
    {
        public int Id { get; set; }
        public int ApprovalHistoryId { get; set; }
        public LicenseApprovalHistory ApprovalHistory { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Comment { get; set; }
        public string CommentedById { get; set; }
        public ApplicationUser CommentedBy { get; set; }
        public DateTime CommentDate { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.LicenseApprovalComment
{
    public class UpdateLicenseApprovalCommentDTO
    {
        public int Id { get; set; }
        public int ApprovalHistoryId { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Comment { get; set; }
        public string CommentedById { get; set; }
        public DateTime CommentDate { get; set; }
    }
}

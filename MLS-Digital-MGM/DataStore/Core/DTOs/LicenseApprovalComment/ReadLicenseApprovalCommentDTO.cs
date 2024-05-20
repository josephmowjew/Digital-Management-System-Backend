using System;
using DataStore.Core.DTOs.User;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.LicenseApprovalComment
{
    public class ReadLicenseApprovalCommentDTO
    {
        public int Id { get; set; }
        public int ApprovalHistoryId { get; set; }
        public DataStore.Core.DTOs.LicenseApprovalHistory.ReadLicenseApprovalHistoryDTO ApprovalHistory { get; set; }
        public string Comment { get; set; }
        public string CommentedById { get; set; }
        public ReadUserDTO CommentedBy { get; set; }
        public DateTime CommentDate { get; set; }
    }
}

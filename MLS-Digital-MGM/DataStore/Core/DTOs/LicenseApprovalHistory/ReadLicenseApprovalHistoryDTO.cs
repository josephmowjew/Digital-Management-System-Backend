using System;
using System.Collections.Generic;
using DataStore.Core.DTOs.LicenseApprovalLevelDTO;
using DataStore.Core.DTOs.User;
using DataStore.Core.Models;

namespace DataStore.Core.DTOs.LicenseApprovalHistory
{
    public class ReadLicenseApprovalHistoryDTO
    {
        public ReadLicenseApprovalHistoryDTO()
        {
            Comments = new List<DataStore.Core.DTOs.LicenseApprovalComment.ReadLicenseApprovalCommentDTO>();
        }

        public int Id { get; set; }
        public int LicenseApplicationId { get; set; }
        public DataStore.Core.DTOs.LicenseApplication.ReadLicenseApplicationDTO LicenseApplication { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ApprovalLevelId { get; set; }
        public ReadLicenseApprovalLevelDTO ApprovalLevel { get; set; }
        public string Status { get; set; }
        public string ChangedById { get; set; }
        public ReadUserDTO ChangedBy { get; set; }
        public virtual List<DataStore.Core.DTOs.LicenseApprovalComment.ReadLicenseApprovalCommentDTO> Comments { get; set; }
    }
}

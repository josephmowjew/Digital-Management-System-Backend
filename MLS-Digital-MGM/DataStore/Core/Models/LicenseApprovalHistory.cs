using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class LicenseApprovalHistory
    {
        public LicenseApprovalHistory()
        {
            Comments = new List<LicenseApprovalComment>();
        }
        
        public int Id { get; set; }
        public int LicenseApplicationId { get; set; }
        public LicenseApplication LicenseApplication { get; set; }
        public DateTime ChangeDate { get; set; }
        public int ApprovalLevelId { get; set; }
        public LicenseApprovalLevel ApprovalLevel { get; set; }
        public string Status { get; set; }
        public string ChangedById { get; set; }
        public ApplicationUser ChangedBy { get; set; }
        public virtual List<LicenseApprovalComment> Comments { get; set; }
    }
}

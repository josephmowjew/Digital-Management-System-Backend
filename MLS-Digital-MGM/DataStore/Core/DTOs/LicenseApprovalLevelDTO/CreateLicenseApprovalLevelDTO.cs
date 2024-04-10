using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs.LicenseApprovalLevelDTO
{
    public class CreateLicenseApprovalLevelDTO
    {

        [Required]
        public int Level { get; set; }
        [Required]
        public int DepartmentId { get; set; }
    }
}

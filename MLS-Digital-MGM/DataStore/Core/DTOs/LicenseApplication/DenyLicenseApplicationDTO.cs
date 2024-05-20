using DataStore.Core.DTOs.LicenseApplication;

namespace DataStore.Core.DTOs.LicenseApplication;

public class DenyLicenseApplicationDTO 
{
    public int LicenseApplicationId { get; set; }
    public string Reason { get; set; }
}

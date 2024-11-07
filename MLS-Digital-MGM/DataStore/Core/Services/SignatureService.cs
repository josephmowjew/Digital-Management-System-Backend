using DataStore.Core.DTOs.User;

namespace DataStore.Core.Services;

public class SignatureService
{
    public static string GenerateSignatureHtml(SignatureDTO data)
    {
        return $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.5;'>
                <p>Regards,</p>
                <div>
                    <span style='color: #1a237e; font-weight: bold;'>{data.Name}</span>
                    <span style='color: #03a9f4;'> | {data.Title}</span>
                </div>
                <div style='margin-top: 10px; border-top: 1px solid #e0e0e0; padding-top: 10px;'>
                    <div style='color: #1a237e; font-weight: bold;'>{data.CompanyName}</div>
                    <div>{data.Address}</div>
                    <div>
                        <span style='color: #03a9f4;'>Tel:</span> {data.Tel} | 
                        <span style='color: #03a9f4;'>Mobile:</span> {data.Mobile} | 
                        <span style='color: #03a9f4;'>Web:</span> <a href='{data.Website}' style='color: #2196f3; text-decoration: none;'>{data.Website}</a>
                    </div>
                </div>
            </div>";
    }
}

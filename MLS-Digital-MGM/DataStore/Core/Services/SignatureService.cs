using DataStore.Core.DTOs.User;
using DataStore.Core.Models;
using DataStore.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataStore.Core.Services;

public class SignatureService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IConfiguration _configuration;
    private readonly string _appUrl;

    public SignatureService(IRepositoryManager repositoryManager, IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _configuration = configuration;
        _appUrl = _configuration.GetValue<string>("AppSettings:APP_URL")?.TrimEnd('/') ?? "";
    }

    public async Task<SignatureDTO> GetGenericSignature()
    {
        var genericSignature = await _repositoryManager.GenericSignatureRepository
            .GetSingleAsync(g => g.IsActive);

        if (genericSignature == null)
            return null;

        var bannerAttachment = genericSignature.Attachments
            .FirstOrDefault(a => a.PropertyName == "Banner");

        return new SignatureDTO
        {
            Name = genericSignature.Name,
            Title = genericSignature.Title,
            CompanyName = genericSignature.CompanyName,
            Address = genericSignature.Address,
            Tel = genericSignature.Tel,
            Mobile = genericSignature.Mobile,
            Website = genericSignature.Website,
            BannerImageUrl = bannerAttachment != null 
                ? $"{_appUrl}/{bannerAttachment.FilePath.Replace("\\", "/")}"
                : null
        };
    }

    public string GenerateSignatureHtml(SignatureDTO data)
    {
        var bannerHtml = "";
        
        if (!string.IsNullOrEmpty(data.BannerImageUrl))
        {
            var imageUrl = data.BannerImageUrl.StartsWith("http") 
                ? data.BannerImageUrl 
                : $"{_appUrl}/{data.BannerImageUrl.TrimStart('/')}";
                
            bannerHtml = $@"<div style='margin-top: 10px;'>
                <img src='{imageUrl}' alt='Company Banner' style='max-width: 600px; width: 100%; height: auto;'/>
            </div>";
        }

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
                {bannerHtml}
            </div>";
    }
}

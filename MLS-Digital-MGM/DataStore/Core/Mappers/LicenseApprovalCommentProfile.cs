using AutoMapper;
using DataStore.Core.Models;
using DataStore.Core.DTOs.LicenseApprovalComment;

namespace DataStore.Core.Mappers
{
    public class LicenseApprovalCommentProfile : Profile
    {
        public LicenseApprovalCommentProfile()
        {
            CreateMap<LicenseApprovalComment, ReadLicenseApprovalCommentDTO>();
            CreateMap<CreateLicenseApprovalCommentDTO, LicenseApprovalComment>();
            CreateMap<UpdateLicenseApprovalCommentDTO, LicenseApprovalComment>();
        }
    }
}

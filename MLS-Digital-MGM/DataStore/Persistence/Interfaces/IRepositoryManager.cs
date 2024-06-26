using DataStore.Core;
using DataStore.Core.Models;
using DataStore.Core.Services.Interfaces;
using DataStore.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }
        // Add other repository properties here, e.g., IProductRepository ProductRepository { get; }
        IUnitOfWork UnitOfWork { get; }

        IDepartmentRepository DepartmentRepository { get; }

        ICountryRepository CountryRepository { get; }

        ITitleRepository TitleRepository { get; }

        IRoleRepository RoleRepository { get; }

        IIdentityTypeRepository IdentityTypeRepository { get; }

        UserManager<ApplicationUser> UserManager { get; }

        IYearOfOperationRepository YearOfOperationRepository { get; }

        ILicenseApprovalLevelRepository LicenseApprovalLevelRepository { get; }

        IFirmRepository FirmRepository { get; }

        IProBonoClientRepository ProBonoClientRepository { get; }

        IProBonoApplicationRepository ProBonoApplicationRepository { get; }

        IAttachmentTypeRepository AttachmentTypeRepository { get; }

        IProBonoRepository ProBonoRepository { get; }

        IProBonoReportRepository ProBonoReportRepository { get; }

        IQualificationTypeRepository QualificationTypeRepository { get; }

        IMemberRepository MemberRepository { get; }

        IMemberQualificationRepository MemberQualificationRepository { get; }

        ILicenseApplicationRepository LicenseApplicationRepository { get; }
        ILicenseRepository LicenseRepository { get; }
        ILicenseApprovalHistoryRepository LicenseApprovalHistoryRepository { get; }
        ICPDTrainingRegistrationRepository CPDTrainingRegistrationRepository {get;}
        ICPDTrainingRepository CPDTrainingRepository {get;}
        ICPDUnitsEarnedRepository CPDUnitsEarnedRepository {get;}
        IPenaltyRepository PenaltyRepository {get;}
        IPenaltyTypeRepository PenaltyTypeRepository {get;}
        IPenaltyPaymentRepository PenaltyPaymentRepository {get;}
        ICommitteeRepository CommitteeRepository {get;}
        ICommitteeMemberRepository CommitteeMemberRepository {get;}
        IMessageRepository MessageRepository {get;}
        IThreadRepository ThreadRepository {get;}
        IInvoiceRequestTypeRepository InvoiceRequestTypeRepository {get;}
        IInvoiceRequestRepository InvoiceRequestRepository {get;}
        IEntityResolverService EntityResolverService { get; } 


    }
}

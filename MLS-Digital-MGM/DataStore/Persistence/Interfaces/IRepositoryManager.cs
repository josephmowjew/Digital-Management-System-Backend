using DataStore.Core.Models;
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

    }
}

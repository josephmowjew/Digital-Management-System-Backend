using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        private IUserRepository _userRepository;
        private IDepartmentRepository _departmentRepository;
        private IErrorLogRepository _errorLogRepository;
        private ICountryRepository _countryRepository;
        private ITitleRepository _titleRepository;
        private IRoleRepository _roleRepository;
        private IIdentityTypeRepository _identityTypeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private IYearOfOperationRepository _yearOfOperationRepository;
        private ILicenseApprovalLevelRepository _licenseApprovalLevelRepository;
        private IFirmRepository _firmRepository;
        private IProBonoClientRepository _proBonoClientRepository;
        private IProBonoApplicationRepository _proBonoApplicationRepository; 
        private IAttachmentTypeRepository _attachmentTypeRepository;
        private IProBonoRepository _proBonoRepository;
        private IProBonoReportRepository _proBonoReportRepository;
        private IQualificationTypeRepository _qualificationTypeRepository;
        private IMemberRepository _memberRepository;
        private IMemberQualificationRepository _memberQualificationRepository;
        private ILicenseApplicationRepository _licenseApplicationRepository;
        private ILicenseRepository _licenseRepository;
        private ILicenseApprovalHistoryRepository _licenseApprovalHistoryRepository;
        private ICPDTrainingRegistrationRepository _CPDTrainingRegistrationRepository;
        private ICPDTrainingRepository _CPDTrainingRepository;
        private ICPDUnitsEarnedRepository _CPDUnitsEarnedRepository;
        private IPenaltyRepository _penaltyRepository;
        private IPenaltyTypeRepository _penaltyTypeRepository;
        private IPenaltyPaymentRepository _penaltyPaymentRepository;

        // Add other repository fields here, e.g., private IProductRepository _productRepository;

       
        public RepositoryManager(ApplicationDbContext context, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context, _unitOfWork, _userManager, _roleManager);

        public IDepartmentRepository DepartmentRepository => _departmentRepository ??= new DepartmentRepository(_context, _unitOfWork);

        public IErrorLogRepository ErrorLogRepository => _errorLogRepository ??= new ErrorLogRepository(_context, _unitOfWork);

        public ICountryRepository CountryRepository => _countryRepository ??= new CountryRepository(_context, _unitOfWork);

        public ITitleRepository TitleRepository => _titleRepository ??= new TitleRepository(_context, _unitOfWork);

        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context, _unitOfWork);

        public IIdentityTypeRepository IdentityTypeRepository => _identityTypeRepository ??= new IdentityTypeRepository(_context, _unitOfWork);

        public IUnitOfWork UnitOfWork => _unitOfWork;

        public UserManager<ApplicationUser> UserManager => _userManager;

        public IYearOfOperationRepository YearOfOperationRepository => _yearOfOperationRepository ??= new YearOfOperationRepository(_context, _unitOfWork);

        public ILicenseApprovalLevelRepository LicenseApprovalLevelRepository => _licenseApprovalLevelRepository ??= new LicenseApprovalLevelRepository(_context, _unitOfWork);
        public IFirmRepository FirmRepository => _firmRepository ??= new FirmRepository(_context, _unitOfWork);

        public IProBonoClientRepository ProBonoClientRepository => _proBonoClientRepository ??= new ProBonoClientRepository(_context, _unitOfWork);

        public IProBonoApplicationRepository ProBonoApplicationRepository => _proBonoApplicationRepository ??= new ProBonoApplicationRepository(_context, _unitOfWork);

        public IAttachmentTypeRepository AttachmentTypeRepository => _attachmentTypeRepository ??= new AttachmentTypeRepository(_context, _unitOfWork);

        public IProBonoRepository ProBonoRepository => _proBonoRepository ??= new ProBonoRepository(_context, _unitOfWork);

        public IProBonoReportRepository ProBonoReportRepository => _proBonoReportRepository ??= new ProBonoReportRepository(_context, _unitOfWork);

        public IQualificationTypeRepository QualificationTypeRepository => _qualificationTypeRepository ??= new QualificationTypeRepository(_context, _unitOfWork);

        public IMemberRepository MemberRepository => _memberRepository ??= new MemberRepository(_context, _unitOfWork);

        public IMemberQualificationRepository MemberQualificationRepository => _memberQualificationRepository ??= new MemberQualificationRepository(_context, _unitOfWork);

        public ILicenseApplicationRepository LicenseApplicationRepository => _licenseApplicationRepository ??= new LicenseApplicationRepository(_context, _unitOfWork);
        public ILicenseRepository LicenseRepository => _licenseRepository ??= new LicenseRepository(_context, _unitOfWork);
        public ILicenseApprovalHistoryRepository LicenseApprovalHistoryRepository => _licenseApprovalHistoryRepository ??= new LicenseApprovalHistoryRepository(_context, _unitOfWork);
        public ICPDTrainingRegistrationRepository CPDTrainingRegistrationRepository => _CPDTrainingRegistrationRepository ??= new CPDTrainingRegistrationRepository(_context, _unitOfWork);
        public ICPDTrainingRepository CPDTrainingRepository => _CPDTrainingRepository ??= new CPDTrainingRepository(_context, _unitOfWork);
        public ICPDUnitsEarnedRepository CPDUnitsEarnedRepository => _CPDUnitsEarnedRepository ??= new CPDUnitsEarnedRepository(_context, _unitOfWork);
        public IPenaltyRepository PenaltyRepository => _penaltyRepository ??= new PenaltyRepository(_context, _unitOfWork);
        public IPenaltyTypeRepository PenaltyTypeRepository => _penaltyTypeRepository ??= new PenaltyTypeRepository(_context, _unitOfWork);
        public IPenaltyPaymentRepository PenaltyPaymentRepository => _penaltyPaymentRepository ??= new PenaltyPaymentRepository(_context, _unitOfWork);

       
    }
}

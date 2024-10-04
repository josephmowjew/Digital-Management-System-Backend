
using DataStore.Core.Models;
using DataStore.Core.Services;
using DataStore.Core.Services.Interfaces;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DataStore.Persistence.SQLRepositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEntityResolverService _entityResolverService;
        private IUserRepository _userRepository;
        private IDepartmentRepository _departmentRepository;
        private IErrorLogRepository _errorLogRepository;
        private ICountryRepository _countryRepository;
        private ITitleRepository _titleRepository;
        private IRoleRepository _roleRepository;
        private IIdentityTypeRepository _identityTypeRepository;
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
        private ICommitteeRepository _committeeRepository;
        private ICommitteeMemberRepository _committeeMemberRepository;
        private IMessageRepository _messageRepository;
        private IThreadRepository _threadRepository;
        private ICustomerRepository _customerRepository;
        private IPaymentRepository paymentRepository;
        private IInvoiceRepository _invoiceRepository;
        private IReceiptRepository _receiptRepository;
        private IQBCustomerRepository _qBCustomerRepository;
        private IQBInvoiceRepository _qBInvoiceRepository;
        //private IInvoiceRequestTypeRepository _invoiceRequestTypeRepository;
        private IInvoiceRequestRepository _invoiceRequestRepository;
        private ILevyDeclarationRepository _levyDeclarationRepository;
        private ILevyPercentRepository _levyPercentRepository;
        private ICommunicationMessageRepository _communicationMessageRepository;
        private ISubcommitteeMembershipRepository _subcommitteeMembershipRepository;
        private ISubcommitteeRepository _subcommitteeRepository;
        private ISubcommitteeThreadRepository _subcommitteeThreadRepository;
        private ISubcommitteeMessageRepository _subcommitteeMessageRepository;
        private IStampRepository _stampRepository;
        private ISignatureRepository _signatureRepository;
        private IEmailQueueRepository _emailQueueRepository;
        private INotaryPublicRepository _notaryPublicRepository;


        public RepositoryManager(
            ApplicationDbContext context, 
            IUnitOfWork unitOfWork, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<Role> roleManager,
            IEntityResolverService entityResolverService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _entityResolverService = entityResolverService;
           
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
        public ICommitteeRepository CommitteeRepository => _committeeRepository ??= new CommitteeRepository(_context, _unitOfWork);
        public ICommitteeMemberRepository CommitteeMemberRepository => _committeeMemberRepository ??= new CommitteeMemberRepository(_context, _unitOfWork);
        public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_context, _unitOfWork);
        public IThreadRepository ThreadRepository => _threadRepository ??= new ThreadRepository(_context, _unitOfWork);
        //public IInvoiceRequestTypeRepository InvoiceRequestTypeRepository => _invoiceRequestTypeRepository ??= new InvoiceRequestTypeRepository(_context, _unitOfWork);
        public IInvoiceRequestRepository InvoiceRequestRepository => _invoiceRequestRepository ??= new InvoiceRequestRepository(_context, _unitOfWork);
        public IEntityResolverService EntityResolverService => _entityResolverService;
        public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_context, _unitOfWork);
        public IPaymentRepository PaymentRepository => paymentRepository ??= new PaymentRepository(_context, _unitOfWork);
        public IInvoiceRepository InvoiceRepository => _invoiceRepository ??= new InvoiceRepository(_context, _unitOfWork);
        public IReceiptRepository ReceiptRepository => _receiptRepository ??= new ReceiptRepository(_context, _unitOfWork);
        public IQBCustomerRepository QBCustomerRepository => _qBCustomerRepository ??= new QBCustomerRepository(_context, _unitOfWork);
        public IQBInvoiceRepository QBInvoiceRepository => _qBInvoiceRepository ??= new QBInvoiceRepository(_context, _unitOfWork);
        public ILevyDeclarationRepository LevyDeclarationRepository => _levyDeclarationRepository ??= new LevyDeclarationRepository(_context, _unitOfWork);
        public ILevyPercentRepository LevyPercentRepository => _levyPercentRepository ??= new LevyPercentRepository(_context, _unitOfWork);
        public ICommunicationMessageRepository CommunicationMessageRepository => _communicationMessageRepository ??= new CommunicationMessageRepository(_context, _unitOfWork);
        public ISubcommitteeMembershipRepository SubcommitteeMembershipRepository => _subcommitteeMembershipRepository ??= new SubcommitteeMembershipRepository(_context, _unitOfWork);
        public ISubcommitteeRepository SubcommitteeRepository => _subcommitteeRepository ??= new SubcommitteeRepository(_context, _unitOfWork);
        public ISubcommitteeThreadRepository SubcommitteeThreadRepository => _subcommitteeThreadRepository ??= new SubcommitteeThreadRepository(_context, _unitOfWork);
        public ISubcommitteeMessageRepository SubcommitteeMessageRepository => _subcommitteeMessageRepository ??= new SubcommitteeMessageRepository(_context, _unitOfWork);
        public IStampRepository StampRepository => _stampRepository ??= new StampRepository(_context, _unitOfWork); 
        public ISignatureRepository SignatureRepository => _signatureRepository ??= new SignatureRepository(_context, _unitOfWork);
        public IEmailQueueRepository EmailQueueRepository => _emailQueueRepository ??= new EmailQueueRepository(_context, _unitOfWork);
        public INotaryPublicRepository NotaryPublicRepository => _notaryPublicRepository ??= new NotaryPublicRepository(_context, _unitOfWork);
    }
}

using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;

namespace DataStore.Persistence.SQLRepositories
{
    public class LicenseApprovalLevelRepository : Repository<LicenseApprovalLevel>, ILicenseApprovalLevelRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public LicenseApprovalLevelRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }
    }
}
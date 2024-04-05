﻿using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
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

        // Add other repository fields here, e.g., private IProductRepository _productRepository;

        public RepositoryManager(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context, _unitOfWork);

        public IDepartmentRepository DepartmentRepository => _departmentRepository ??= new DepartmentRepository(_context, _unitOfWork);

        public IErrorLogRepository ErrorLogRepository => _errorLogRepository ??= new ErrorLogRepository(_context, _unitOfWork);

        public ICountryRepository CountryRepository => _countryRepository ??= new CountryRepository(_context, _unitOfWork);

        public ITitleRepository TitleRepository => _titleRepository ??= new TitleRepository(_context, _unitOfWork);

        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context, _unitOfWork);

        public IIdentityTypeRepository IdentityTypeRepository => _identityTypeRepository ??= new IdentityTypeRepository(_context, _unitOfWork);

        public IUnitOfWork UnitOfWork => _unitOfWork;
    }
}

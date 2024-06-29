using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStore.Persistence.SQLRepositories
{
    public class CustomerRepository : Repository<QBCustomer>, ICustomerRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
    }
}

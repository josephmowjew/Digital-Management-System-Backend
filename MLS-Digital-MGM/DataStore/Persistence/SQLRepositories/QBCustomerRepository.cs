using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class QBCustomerRepository : Repository<QBCustomer>, IQBCustomerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public QBCustomerRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<QBCustomer>> GetAllCustomersAsync(CursorParams cursorParams)
        {
          IQueryable<QBCustomer> query = _context.Set<QBCustomer>();

        if (!string.IsNullOrEmpty(cursorParams.SearchTerm))
        {
            query = query.Where(x => x.CustomerName.Contains(cursorParams.SearchTerm) || 
                                    x.FirstName.Contains(cursorParams.SearchTerm) ||
                                    x.Id.Contains(cursorParams.SearchTerm) ||
                                    x.LastName.Contains(cursorParams.SearchTerm) ||
                                    x.CompanyName.Contains(cursorParams.SearchTerm) ||
                                    x.EmailAddress.Contains(cursorParams.SearchTerm) || 
                                    x.PhoneNumber.Contains(cursorParams.SearchTerm));
        }

       
        if (cursorParams.Take > 0)
        {
            query = query.Skip(cursorParams.Skip).Take(cursorParams.Take);
        }

            return await query.ToListAsync();
        }
    }
}

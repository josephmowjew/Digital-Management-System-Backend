using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class QBInvoiceRepository : Repository<QBInvoice>, IQBInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public QBInvoiceRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<QBInvoice>> GetAllInvoicesAsync(CursorParams cursorParams)
        {
          IQueryable<QBInvoice> query = _context.Set<QBInvoice>();

        if (!string.IsNullOrEmpty(cursorParams.SearchTerm))
        {
            query = query.Where(x => x.CustomerName.Contains(cursorParams.SearchTerm) || 
                                    x.InvoiceNumber.Contains(cursorParams.SearchTerm) ||
                                    x.Id.Contains(cursorParams.SearchTerm) ||
                                    x.InvoiceDate.ToString("yyyy-MM-dd").Contains(cursorParams.SearchTerm) ||
                                    x.InvoiceAmount.ToString("F2").Contains(cursorParams.SearchTerm) ||
                                    x.UnpaidAmount.ToString("F2").Contains(cursorParams.SearchTerm) ||
                                    x.InvoiceType.Contains(cursorParams.SearchTerm) ||
                                    x.CustomerId.Contains(cursorParams.SearchTerm) ||
                                    x.InvoiceDescription.Contains(cursorParams.SearchTerm));
        }

       
        if (cursorParams.Take > 0)
        {
            query = query.Skip(cursorParams.Skip).Take(cursorParams.Take);
        }

            return await query.ToListAsync();
        }
    }
}

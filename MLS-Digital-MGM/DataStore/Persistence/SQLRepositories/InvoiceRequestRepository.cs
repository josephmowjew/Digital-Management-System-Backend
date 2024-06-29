using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class InvoiceRequestRepository : Repository<InvoiceRequest>, IInvoiceRequestRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceRequestRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<InvoiceRequest> GetByIdAsync(int id)
        {
            return await _context.InvoiceRequests.Include(ir => ir.Customer).Include(ir => ir.YearOfOperation).Include(ir => ir.QBInvoice).Include(ir => ir.CreatedBy).FirstOrDefaultAsync(ir => ir.Id == id);
        }
    }
}

using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
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
            return await _context.InvoiceRequests
                .Include(ir => ir.Customer)
                .Include(ir => ir.YearOfOperation)
                .Include(ir => ir.QBInvoice)
                .Include(ir => ir.CreatedBy)
                .Include(ir => ir.Attachments)
                    .ThenInclude(a => a.AttachmentType)
                .FirstOrDefaultAsync(ir => ir.Id == id);
        }
        
        public async Task<QBInvoice> GetQBInvoiceByIdAsync(string id){
            return await _context.QBInvoices.FindAsync(id);
        }

        public async Task<int> GetPendingInvoiceRequestsCountAsync(){
            return await _context.InvoiceRequests.CountAsync(ir => ir.Status == Lambda.Pending);
        }
        public async Task<IEnumerable<InvoiceRequest>> GetInvoiceRequestsForReportAsync(DateTime startDate, DateTime endDate, string referencedEntityType = null)
        {
            var query = _context.InvoiceRequests
                .Where(ir => ir.CreatedDate >= startDate && ir.CreatedDate <= endDate);

            if (!string.IsNullOrEmpty(referencedEntityType))
            {
                query = query.Where(ir => ir.ReferencedEntityType == referencedEntityType);
            }

            return await query.ToListAsync();
        }
    }
}

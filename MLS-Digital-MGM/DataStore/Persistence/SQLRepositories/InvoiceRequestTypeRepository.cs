// using DataStore.Core.Models;
// using DataStore.Data;
// using DataStore.Persistence.Interfaces;
// using Microsoft.EntityFrameworkCore;
// using System.Threading.Tasks;

// namespace DataStore.Persistence.SQLRepositories
// {
//     public class InvoiceRequestTypeRepository : Repository<InvoiceRequestType>, IInvoiceRequestTypeRepository
//     {
//         protected readonly ApplicationDbContext _context;
//         private readonly IUnitOfWork _unitOfWork;

//         public InvoiceRequestTypeRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
//         {
//             this._context = context;
//             this._unitOfWork = unitOfWork;
//         }

//         public async Task<InvoiceRequestType> GetByIdAsync(int id)
//         {
//             return await _context.InvoiceRequestTypes.FirstOrDefaultAsync(i => i.Id == id);
//         }
//     }
// }

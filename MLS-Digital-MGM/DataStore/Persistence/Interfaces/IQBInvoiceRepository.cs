using DataStore.Core.Models;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IQBInvoiceRepository : IRepository<QBInvoice>
    {
       Task<IEnumerable<QBInvoice>> GetAllInvoicesAsync(CursorParams cursorParams);
    }
}

using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface IInvoiceRequestRepository : IRepository<InvoiceRequest>
    {
        // Additional methods specific to the InvoiceRequest entity, if needed
        Task<QBInvoice> GetQBInvoiceByIdAsync(string id);

        Task<int> GetPendingInvoiceRequestsCountAsync();

        Task<IEnumerable<InvoiceRequest>> GetInvoiceRequestsForReportAsync(DateTime startDate, DateTime endDate, string referencedEntityType = null);

    }
}
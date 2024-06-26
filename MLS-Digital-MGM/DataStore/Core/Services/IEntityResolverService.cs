using DataStore.Core.Models;

namespace DataStore.Core.Services.Interfaces
{
    public interface IEntityResolverService
    {
        object GetReferencedEntity(InvoiceRequest invoiceRequest);
    }
}

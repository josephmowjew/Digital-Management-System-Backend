using DataStore.Core.Models;
using MLS_Digital_MGM.DataStore.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IQBCustomerRepository : IRepository<QBCustomer>
    {
       Task<IEnumerable<QBCustomer>> GetAllCustomersAsync(CursorParams cursorParams);
    }
}

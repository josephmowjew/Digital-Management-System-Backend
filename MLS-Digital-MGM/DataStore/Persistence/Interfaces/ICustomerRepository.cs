using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface ICustomerRepository : IRepository<QBCustomer>
    {
        // Additional methods specific to the Customer entity, if needed
    }
}

using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface IPaymentRepository : IRepository<QBPayment>
    {
        // Additional methods specific to the Payment entity, if needed
    }
}

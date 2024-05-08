using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IYearOfOperationRepository : IRepository<YearOfOperation>
    {
        // Additional methods specific to the YearOfOperation entity, if needed

        Task<YearOfOperation> GetCurrentYearOfOperation();
    }
}
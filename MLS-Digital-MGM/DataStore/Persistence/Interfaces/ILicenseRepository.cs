using DataStore.Core.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using License = DataStore.Core.Models.License;

namespace DataStore.Persistence.Interfaces
{
    public interface ILicenseRepository : IRepository<License>
    {
        // Additional methods specific to the License entity, if needed
        Task<License?> GetLastLicenseNumber(int id);
        Task<License?> GetSingleAsync(Expression<Func<License, bool>> predicate);
    }
}

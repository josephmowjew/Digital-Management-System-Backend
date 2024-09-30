using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataStore.Persistence.Interfaces
{
    public interface IUnitOfWork: IAsyncDisposable
    {
       Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CommitAsync();
        Task RollbackAsync();

        Task<int> SaveChangesAsync();
    }
}

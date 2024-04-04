using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Rollback();
    }
}

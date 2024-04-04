using DataStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository UserRepository { get; }
        // Add other repository properties here, e.g., IProductRepository ProductRepository { get; }
        IUnitOfWork UnitOfWork { get; }

        IDepartmentRepository DepartmentRepository { get; }

        ICountryRepository CountryRepository { get; }



    }
}

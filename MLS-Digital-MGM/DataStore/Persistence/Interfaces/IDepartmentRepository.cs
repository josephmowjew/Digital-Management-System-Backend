using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IDepartmentRepository: IRepository<Department>
    {
        // Additional methods specific to the Department entity, if needed
    }
}

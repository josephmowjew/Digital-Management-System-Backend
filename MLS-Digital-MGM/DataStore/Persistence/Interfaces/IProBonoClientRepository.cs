using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IProBonoClientRepository : IRepository<ProbonoClient>
    {
        // Additional methods specific to the ProBonoClient entity, if needed

        Task<int> GetProBonoClientCount();
    }
}

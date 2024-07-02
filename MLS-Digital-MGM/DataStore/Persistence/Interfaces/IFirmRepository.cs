using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IFirmRepository : IRepository<Firm>
    {
        // Additional methods specific to the Firm entity, if needed
        Task<int> GetFirmsCountAsync();
    }
}
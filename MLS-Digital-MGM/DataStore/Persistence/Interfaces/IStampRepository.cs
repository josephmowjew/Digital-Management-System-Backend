
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IStampRepository : IRepository<Stamp>
    {
        Task<List<Stamp>> GetStampsAsync();
        Task<Stamp> GetStampByIdAsync(int stampId);
        Task<Stamp> GetStampByNameAsync(string stampName);
    }
}


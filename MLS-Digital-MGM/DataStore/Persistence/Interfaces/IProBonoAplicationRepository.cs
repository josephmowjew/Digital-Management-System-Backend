using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IProBonoApplicationRepository : IRepository<ProBonoApplication>
    {
        // Additional methods specific to the ProBonoApplication entity, if needed
        Task<int> GetProBonoApplicationsCountByUserAsync(string createdById);
    }
}

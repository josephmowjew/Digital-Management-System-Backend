using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface IProBonoReportRepository : IRepository<ProBonoReport>
    {
        // Additional methods specific to the ProBonoReport entity, if needed
        Task<double> GetProBonoHoursTotalByUserAsync(string memberId);
    }
}

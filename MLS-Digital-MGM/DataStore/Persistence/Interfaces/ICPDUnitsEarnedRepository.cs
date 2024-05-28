using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface ICPDUnitsEarnedRepository : IRepository<CPDUnitsEarned>
    {
        // Additional methods specific to the CPDUnitsEarned entity, if needed
        Task<int> GetSummedCPDUnitsEarnedByMemberId(int memberId);
    }
}

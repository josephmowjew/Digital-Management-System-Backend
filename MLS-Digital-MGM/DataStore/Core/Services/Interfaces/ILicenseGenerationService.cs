using System.Threading.Tasks;
using DataStore.Core.Models;

namespace DataStore.Core.Services.Interfaces
{
    public interface ILicenseGenerationService
    {
        Task<(int success, int failed)> GenerateLicensesForYear(int? yearOfOperationId = null);
    }
}


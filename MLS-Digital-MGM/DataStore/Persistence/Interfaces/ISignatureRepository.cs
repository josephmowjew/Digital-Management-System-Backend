
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface ISignatureRepository : IRepository<Signature>
    {
        Task<List<Signature>> GetSignaturesAsync();
        Task<Signature> GetSignatureByIdAsync(int signatureId);
        Task<Signature?> GetSignatureByNameAsync(string signatureName);
    }
}

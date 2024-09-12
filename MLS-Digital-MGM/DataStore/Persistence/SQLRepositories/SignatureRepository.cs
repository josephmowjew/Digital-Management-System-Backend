
using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class SignatureRepository : Repository<Signature>, ISignatureRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SignatureRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<Signature>> GetSignaturesAsync()
        {
            return await _context.Signatures.ToListAsync();
        }

        public async Task<Signature> GetSignatureByIdAsync(int signatureId)
        {
            return await _context.Signatures
            .Include(s => s.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .Include(s => s.YearOfOperation)
            .Include(s => s.CreatedBy)
            .FirstOrDefaultAsync(s => s.Id == signatureId);
        }

        public Task<Signature?> GetSignatureByNameAsync(string signatureName)
        {

            string signatureNameToLower = signatureName.ToLower().Trim();

            return _context.Signatures.Include(s => s.Attachments).ThenInclude(t => t.AttachmentType).FirstOrDefaultAsync(x => x.Name.Trim().ToLower() == signatureNameToLower);
        }
    }
}

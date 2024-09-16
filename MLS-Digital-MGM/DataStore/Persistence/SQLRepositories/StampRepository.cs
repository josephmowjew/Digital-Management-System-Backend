
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
    public class StampRepository : Repository<Stamp>, IStampRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public StampRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<List<Stamp>> GetStampsAsync()
        {
            return await _context.Stamps.ToListAsync();
        }

        public async Task<Stamp> GetStampByIdAsync(int stampId)
        {
            return await _context.Stamps
            .Include(t => t.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .Include(t => t.YearOfOperation)
            .FirstOrDefaultAsync(t => t.Id == stampId);
        }

        public Task<Stamp> GetStampByNameAsync(string stampName)
        {
            return _context.Stamps
            .Include(t => t.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .Include(t => t.YearOfOperation)
            .FirstOrDefaultAsync(x => x.Name == stampName);
        }
    }
}

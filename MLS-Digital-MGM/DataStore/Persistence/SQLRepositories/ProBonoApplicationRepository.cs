using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class ProBonoApplicationRepository : Repository<ProBonoApplication>, IProBonoApplicationRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ProBonoApplicationRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ProBonoApplication> GetByIdAsync(int id)
        {
            return await _context.ProBonoApplications
            .Include(t => t.CreatedBy)
            .Include(t => t.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .Include(t => t.YearOfOperation)
            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> GetProBonoApplicationsCountByUserAsync(string userId)
        {
            return await _context.ProBonoApplications
                .Where(p => p.CreatedById == userId && p.Status != Lambda.Deleted && p.ApplicationStatus != Lambda.Approved).CountAsync(); ;
        }


    }
}

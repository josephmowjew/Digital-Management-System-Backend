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
    public class ProBonoReportRepository : Repository<ProBonoReport>, IProBonoReportRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ProBonoReportRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ProBonoReport> GetByIdAsync(int id)
        {
            return await _context.ProBonoReports
            .Include(t => t.CreatedBy)
            .Include(t => t.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<double> GetProBonoHoursTotalByUserAsync(string memberId)
        {
            return await _context.ProBonoReports
                .Where(p => p.CreatedById == memberId && p.Status != Lambda.Deleted && p.ReportStatus == Lambda.Approved)
                .SumAsync(p => p.ProBonoHours);
        }
    }
}

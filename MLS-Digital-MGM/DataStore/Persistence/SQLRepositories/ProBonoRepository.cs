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
    public class ProBonoRepository : Repository<ProBono>, IProBonoRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ProBonoRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ProBono?> GetLastProBonoAsync()
        {
            return await _context.ProBonos
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetProBonosCountAsync(){return await _context.ProBonos
            .Where(pb => pb.Status == Lambda.Active)
            .CountAsync();
        }
    }
}

using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class ProBonoClientRepository : Repository<ProbonoClient>, IProBonoClientRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ProBonoClientRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task DeleteAsync(ProbonoClient entity)
        {

            if (entity != null)
            {
                entity.DeletedDate = DateTime.Now;
                entity.Status = Lambda.Deleted;

                // Mark related ProbonoApplications as deleted
                var applications = _context.Set<ProBono>()
                                           .Where(app => app.ProbonoClientId == entity.Id && app.Status != "Deleted")
                                           .ToList();

                foreach (var app in applications)
                {
                    app.DeletedDate = DateTime.Now;
                    app.Status = "Deleted";
                }
            }
        }
    }

}

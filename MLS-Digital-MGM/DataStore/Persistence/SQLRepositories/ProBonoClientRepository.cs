using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using DataStore.Core.Models;

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

                // Mark related Probonocases as deleted
                var cases = _context.Set<ProBono>()
                                           .Where(app => app.ProbonoClientId == entity.Id && app.Status != "Deleted")
                                           .ToList();

                // Mark related ProbonoApplications as deleted
                var applications = _context.Set<ProBonoApplication>()
                                           .Where(app => app.ProbonoClientId == entity.Id && app.Status != "Deleted")
                                           .ToList();

                if (cases.Any())
                {
                    foreach (var probono in cases)
                    {
                        probono.DeletedDate = DateTime.Now;
                        probono.Status = Lambda.Deleted;
                    }
                }
                
                if(applications.Any())
                {
                    foreach (var app in applications)
                    {
                        app.DeletedDate = DateTime.Now;
                        app.Status = "Deleted";
                    }
                }
            }
        }
    
        public async Task<int> GetProBonoClientCount(){
            return await _context.ProbonoClients.CountAsync(c => c.Status == Lambda.Active);
        }

        public async Task<int> GetProBonoDeleteRequestedClientCount(){
            return await _context.ProbonoClients.CountAsync(c => c.deleteRequest == true);
        }
    }

}

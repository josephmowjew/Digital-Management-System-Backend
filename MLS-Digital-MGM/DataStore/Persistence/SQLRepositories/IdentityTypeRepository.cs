using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class IdentityTypeRepository: Repository<IdentityType>, IIdentityTypeRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public IdentityTypeRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        // Implement additional methods here for identity type specific operations
    }
}
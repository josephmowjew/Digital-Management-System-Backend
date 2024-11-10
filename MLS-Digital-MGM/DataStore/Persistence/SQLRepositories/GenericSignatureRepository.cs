using DataStore.Core.DTOs.User;
using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;

namespace DataStore.Persistence.SQLRepositories
{
    public class GenericSignatureRepository : Repository<GenericSignature>, IGenericSignatureRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public GenericSignatureRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }
    }
}

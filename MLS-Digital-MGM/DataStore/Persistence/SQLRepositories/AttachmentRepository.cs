using DataStore.Data;
using DataStore.Core.Models;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataStore.Persistence.SQLRepositories
{
    public class AttachmentRepository : Repository<Attachment>, IAttachmentRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AttachmentRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }
    }
}

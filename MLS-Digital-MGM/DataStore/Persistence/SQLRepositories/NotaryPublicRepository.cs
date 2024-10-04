using DataStore.Core.Models;
using DataStore.Core.Models.Interfaces;
using DataStore.Data;
using DataStore.Helpers;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class NotaryPublicRepository : Repository<NotaryPublic>, INotaryPublicRepository
    {
        protected readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public NotaryPublicRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
            this._context = context;
            this._unitOfWork = unitOfWork;
        }

        public async Task<NotaryPublic> GetByIdAsync(int id)
        {
            return await _context.NotariesPublic
            .Include(t => t.Member)
            .Include(t => t.YearOfOperation)
            .Include(t => t.Attachments)
            .ThenInclude(t => t.AttachmentType)
            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<NotaryPublic>> GetByMemberId(int memberId)
        {
            return await _context.NotariesPublic.Where(notary => notary.MemberId == memberId && notary.Status != Lambda.Deleted).ToListAsync();
        }

        public async Task<NotaryPublic> DeleteAsync(NotaryPublic notaryPublic)
        {
            if (notaryPublic != null)
            {
                notaryPublic.DeletedDate = DateTime.Now;
                notaryPublic.Status = Lambda.Deleted;

                // Mark related Attachments as deleted
                var attachments = _context.Attachments
                                           .Where(att => att.Id == notaryPublic.Id && att.Status != "Deleted")
                                           .ToList();

                if (attachments.Any())
                {
                    foreach (var attachment in attachments)
                    {
                        attachment.DeletedDate = DateTime.Now;
                        attachment.Status = Lambda.Deleted;
                    }
                }
            }

            return notaryPublic;
        }

        public async Task<int> GetNotaryPublicCountAsync()
        {
            return await _context.NotariesPublic
                .Where(np => np.Status == Lambda.Active)
                .CountAsync();
        }

        public async Task<NotaryPublic> GetNotariesPublicByMemberIdAsync(int memberId)
        {
            return await _context.NotariesPublic
                .Include(t => t.Member)
                .Include(t => t.YearOfOperation)
                .Include(t => t.Attachments)
                .ThenInclude(t => t.AttachmentType)
                .FirstOrDefaultAsync(t => t.MemberId == memberId && t.Status != Lambda.Deleted);
        }

    }
}

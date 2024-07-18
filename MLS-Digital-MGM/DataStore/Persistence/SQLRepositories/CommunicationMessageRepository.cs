using DataStore.Core.Models;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataStore.Persistence.SQLRepositories
{
    public class CommunicationMessageRepository : Repository<CommunicationMessage>, ICommunicationMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public CommunicationMessageRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) 
            : base(context, unitOfWork)
        {
            _context = context;
        }

        public async Task<CommunicationMessage> GetByIdWithRecipientsAsync(int id)
        {
            return await _context.CommunicationMessages
                .Include(cm => cm.SentByUser)
                .FirstOrDefaultAsync(cm => cm.Id == id);
        }
    }
}
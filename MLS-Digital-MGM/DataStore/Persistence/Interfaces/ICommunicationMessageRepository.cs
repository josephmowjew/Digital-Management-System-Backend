using DataStore.Core.Models;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface ICommunicationMessageRepository : IRepository<CommunicationMessage>
    {
        Task<CommunicationMessage> GetByIdWithRecipientsAsync(int id);
    }
}
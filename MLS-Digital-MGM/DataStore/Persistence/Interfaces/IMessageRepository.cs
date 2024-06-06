using DataStore.Core.Models;
using System.Collections.Generic;

namespace DataStore.Persistence.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        // Additional methods specific to the Message entity, if needed
    }
}
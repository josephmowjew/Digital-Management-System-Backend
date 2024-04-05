using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface ITitleRepository : IRepository<Title>
    {
        // Additional methods specific to the Title entity, if needed
    }
}
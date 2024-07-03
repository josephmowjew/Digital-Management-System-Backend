using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces
{
    public interface ICountryRepository: IRepository<Country>
    {
        // Additional methods specific to the Department entity, if needed
        Task<int> GetCountriesCountAsync();
    }
}
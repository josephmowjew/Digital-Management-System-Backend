using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces;

  public interface IMemberRepository : IRepository<Member>
  {
    // Additional methods specific to the Member entity, if needed

     Task<Member?> GetMemberByUserId(string userId);
  
  }

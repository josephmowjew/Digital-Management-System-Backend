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
     
     

     Task<IEnumerable<Member>> GetAllAsync();

     Task<Member?> GetByIdAsync(int id);

     Task<int> GetMembersCountAsync();
     Task<int> GetLicensedMembersCountAsync(int year);
     Task<int> GetUnlicensedMembersCountAsync(int year);
     Task<IEnumerable<Member>> GetLicensedMembersAsync(int year);
     Task<IEnumerable<Member>> GetUnlicensedMembersAsync(int year);
     Task<IEnumerable<Member>> GetMembersByFirmIdAsync(int firmId);
  
  }

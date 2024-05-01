using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Persistence.Interfaces;

    public interface IMemberQualificationRepository : IRepository<MemberQualification>
    {
        // Additional methods specific to the MemberQualification entity, if needed 

        Task<MemberQualification?> GetMemberQualificationByMemberId(int memberId);
    }

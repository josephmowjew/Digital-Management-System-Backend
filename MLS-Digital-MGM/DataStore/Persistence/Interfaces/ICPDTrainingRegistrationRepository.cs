using DataStore.Core.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataStore.Persistence.Interfaces
{
    public interface ICPDTrainingRegistrationRepository : IRepository<CPDTrainingRegistration>
    {
        // Additional methods specific to the CPDTrainingRegistration entity, if needed
       Task <List<CPDTrainingRegistration>> GetAll(Expression<Func<CPDTrainingRegistration, bool>> value);
    }
}

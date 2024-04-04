using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Services.Interfaces
{
    public interface IErrorLogService
    {
        Task LogErrorAsync(Exception ex);
    }
}

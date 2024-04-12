using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models.Interfaces
{
    public interface IOwned
    {
        public ApplicationUser CreatedBy { get; set; }
        public int ProbonoClientId { get; set; }
    }
}

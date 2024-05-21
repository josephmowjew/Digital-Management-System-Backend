using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Models
{
    public class CPDUnitsEarned: Meta
    {
        public int MemberId { get; set; }
        public int CPDTrainingId { get; set; }
        public CPDTraining CPDTraining { get; set; }
        public int UnitsEarned { get; set; }
        public int YearOfOperationId { get; set; }
        public YearOfOperation YearOfOperation { get; set; }
    }
}

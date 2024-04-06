using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.ViewModels
{
    public class PasswordResetModel
    {
        public string Email { get; set; }

        public int Pin { get; set; }

        public string Password { get; set; }
    }
}

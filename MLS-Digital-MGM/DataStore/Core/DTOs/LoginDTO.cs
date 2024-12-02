using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.DTOs
{
    public class LoginDTO
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }

        public string TokenType { get; set; }
        public DateTime TokenExpiryMinutes { get; set; }


        [Column(TypeName = "Date")]
        public DateTime? DateOfBirth { get; set; }
        public string ProfilePicture {get; set; }

    }
}

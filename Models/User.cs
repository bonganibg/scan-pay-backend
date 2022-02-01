using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Models
{
    public class User
    {
        public Guid UserID { get; init; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }        
        public string Password { get; set; }   
    }
}

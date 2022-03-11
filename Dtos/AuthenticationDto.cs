using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class AuthenticationDto
    {
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

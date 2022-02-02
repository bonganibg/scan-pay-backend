using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class GetUserDto
    {
        public string FullName { get; init; }
        public string Username { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
    }
}

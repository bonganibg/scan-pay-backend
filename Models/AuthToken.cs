using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Models
{
    public class AuthToken
    {
        public Guid Token { get; init; }
        public Guid UserID { get; set; }
        public bool isBusiness { get; set; }
        public DateTime Created { get; set; }
    }
}

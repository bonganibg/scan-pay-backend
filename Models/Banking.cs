using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Models
{
    public class Banking
    {
        public Guid CardID { get; init; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public string CardName { get; set; }

    }
}

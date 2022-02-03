using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class BankingDto
    {
        public string CardNumber { get; init; }
        public string ExpiryDate { get; init; }
        public string CVV { get; init; }
        public string CardName { get; init; }
    }
}

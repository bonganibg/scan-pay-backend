using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class CreateBankingDto
    {
        public string holderID { get; init; }
        public bool isBusiness { get; init; }
        public string CardNumber { get; init; }
        public string ExpiryDate { get; init; }
        public string CVV { get; init; }
        public string CardName { get; init; }
    }
}

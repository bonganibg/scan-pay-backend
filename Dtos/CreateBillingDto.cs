using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class CreateBillingDto
    {
        public string holderID { get; init; }
        public bool isBusiness { get; init; }
        public string Country { get; init; }
        public string AddressOne { get; init; }
        public string AddressTwo { get; init; }
        public string City { get; init; }
        public string Zip { get; init; }
        public string State { get; init; }
    }
}

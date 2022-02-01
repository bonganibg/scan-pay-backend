using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Models
{
    public class BillingAddress
    {
        public Guid BillingID { get; init; }
        public string Country { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
    }
}

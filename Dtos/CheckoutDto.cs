using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class CheckoutDto
    {
        public string CustomerID { get; set; }
        public string StoreID { get; set; }
    }
}

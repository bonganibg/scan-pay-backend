using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Models
{
    public class Store
    {
        public Guid StoreId { get; init; }
        public Guid BusinessID { get; init; }
        public string Name { get; set; }
        public string QrCode { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Models
{
    public class Store
    {
        public Guid StoreId { get; init; }
        public Guid BusinessID { get; set; }
        public string Name { get; init; }
        public string QrCode { get; init; }

    }
}

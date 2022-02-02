using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class GetStoreDto
    {
        public string StoreId { get; init; }        
        public string Name { get; init; }
        public string QrCode { get; init; }
    }
}

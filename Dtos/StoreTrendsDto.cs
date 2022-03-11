using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class StoreTrendsDto
    {
        public string StoreID { get; set; }
        public string Name { get; set; }
        public string ImageUri { get; set; }
        public decimal Trend { get; set; }
        public int Sales { get; set; }
    }
}

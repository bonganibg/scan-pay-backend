using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Models
{
    public class Product
    {
        public Guid ProductID { get; init; }
        public string Barcodes { get; init; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }
        public decimal SalePrice { get; set; }
        public bool Sale { get; set; }
        public int Stock { get; set; }
    }
}

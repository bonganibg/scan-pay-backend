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
        public string Name { get; init; }
        public decimal Price { get; init; }
        public string Description { get; init; }
        public string ImageUri { get; init; }
        public decimal SalePrice { get; init; }
        public bool Sale { get; init; }
        public int Stock { get; init; }
    }
}

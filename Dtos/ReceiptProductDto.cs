using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class ReceiptProductDto
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public bool Sale { get; set; }
        public decimal TotalPrice { get; set; }

    }
}

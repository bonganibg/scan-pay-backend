using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Models
{
    public class Receipt
    {
        public Guid ReceiptID { get; init; }
        public Guid StoreID { get; init; }
        public Guid UserID { get; init; }
        public string QrCode { get; init; }
        public decimal TotalPrice { get; init; }
        public DateTimeOffset Date { get; init; }
        public List<Product> Products { get; init; }
    }
}

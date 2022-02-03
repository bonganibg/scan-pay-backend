using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class CartDto
    {
        [Required]
        public string CustomerID { get; init; }
        [Required]
        public string ProductID { get; init; }
        [Required]
        public string StoreID { get; init; }
        [Required]
        public int Quantity { get; set; }

    }
}

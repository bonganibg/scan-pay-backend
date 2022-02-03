using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class CreateProductDto
    {
        [Required]
        public string StoreID { get; set; }
        [Required]
        public string Name { get; init; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public decimal Price { get; init; }
        public string Description { get; init; }
        public string ImageUri { get; init; }
        public decimal SalePrice { get; init; }
        public bool Sale { get; init; }
        [Required]
        public int Stock { get; init; }
    }
}

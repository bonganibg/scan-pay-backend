using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class UpdateCartDto
    {
        [Required]
        public string CartID { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}

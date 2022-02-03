using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class RemoveItemDto
    {
        [Required]
        public string CartID { get; init; }
        [Required]
        public string ProductID { get; init; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class StoreDto
    {      
        public string BusinessID { get; set; }
        [Required]
        public string Name { get; init; }        
    }
}

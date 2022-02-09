using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Dtos
{
    public class UpdateBusinessDto
    {
        public string BusinessID { get; set; }
        [Required]
        public string Name { get; init; }
        [Required]
        public string Email { get; init; }        
        public string PhoneNumber { get; init; }
        [Required]
        public string Password { get; init; }
    }
}

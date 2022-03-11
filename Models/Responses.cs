using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Models
{
    public class Response
    {
        public string Message { get; set; }
        public IEnumerable<dynamic> Data { get; set; }
    }

}

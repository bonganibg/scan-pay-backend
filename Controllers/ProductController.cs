using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScanPayAPI.Dtos;
using ScanPayAPI.Models;
using ScanPayAPI.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ProductRepository prodRepo = new ProductRepository();

        [HttpPost]
        public string CreateProduct(CreateProductDto product)
        {
            bool result = prodRepo.CreateNewProduct(product);

            if (result)
                return "Added";
            return "Not added";
        }

        [HttpGet("{qr}")]
        public Product Get(string qr)
        {
            return prodRepo.GetProduct(qr);
        }

    }
}

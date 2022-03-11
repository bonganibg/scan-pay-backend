using Microsoft.AspNetCore.Cors;
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
    [EnableCors]
    public class ProductController : ControllerBase
    {
        ProductRepository prodRepo = new ProductRepository();


        [HttpGet("test")]
        [EnableCors("BusinessApp")]
        public ActionResult<ReceiptProductDto> Test()
        {
            ReceiptProductDto dummy =  new ReceiptProductDto {
                Name = "Testing",
                Price = 45,
                ProductID = "ghfoisdguofgasdf",
                Quantity = 10,
                Sale = false,
                SalePrice = 20,
                TotalPrice = 2000           
            };

            return Ok(dummy);
        }

        [HttpPost]
        [EnableCors("BusinessApp")]
        public string CreateProduct(CreateProductDto product)
        {
            bool result = prodRepo.CreateNewProduct(product);

            if (result)
                return "Added";
            return "Not added";
        }

        [HttpGet("{qr}")]
        [EnableCors("BusinessApp")]
        public Product Get(string qr)
        {
            return prodRepo.GetProduct(qr);
        }


        //Get a list of products in a store
        [HttpGet("store/{id}")]
        [EnableCors("BusinessApp")]
        public List<Product> GetProduct(string id)
        {
            List<Product> prods = prodRepo.GetProducts(id);

            return prods;
        }

        [HttpPut]
        public string Update(Product prod)
        {
            if (prodRepo.UpdateProduct(prod))
                return "Updated";
            else
                return "Failed to update";
        }

    }
}

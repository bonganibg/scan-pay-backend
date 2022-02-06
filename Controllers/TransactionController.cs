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
    public class TransactionController : ControllerBase
    {

        TransactionRepository tranRepo = new();

        #region Shopping Cart
        
        [HttpPost("cart")]
        public string AddProductToCart([FromBody] CartDto cartInfo)
        {
            if (tranRepo.AddProductToCart(cartInfo))
                return "Item added to cart";
            else
                return "Item not added to cart";
        }

        [HttpGet("cart/{id}")]
        public List<Product> GetCartProducts(string id)
        {
            return tranRepo.GetCartItems(id);
        }

        [HttpDelete("cart")]
        public string RemoveProduct(RemoveItemDto remove)
        {
            if (tranRepo.RemoveItemFromCart(remove))
                return "Product Removed";
            else
                return "Failed to remove item";
        }

        [HttpPut("cart")]
        public string UpdateCart(UpdateCartDto cart)
        {
            if (tranRepo.UpdateShoppingCart(cart))
                return "Updated";
            else
                return "Not Updated";
        }

        #endregion

        #region Checkout 

        [HttpGet("checkout")]
        public Receipt CompleteCheckout([FromBody] CheckoutDto checkout)
        {
            return tranRepo.Checkout(checkout);
        }

        #endregion

    }
}

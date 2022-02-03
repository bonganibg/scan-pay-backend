using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScanPayAPI.Dtos;
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

        [HttpPost]
        public string AddProductToCart([FromBody] CartDto cartInfo)
        {
            if (tranRepo.AddProductToCart(cartInfo))
                return "Item added to cart";
            else
                return "Item not added to cart";
        }

        [HttpDelete]
        public string RemoveProduct(RemoveItemDto remove)
        {
            if (tranRepo.RemoveItemFromCart(remove))
                return "Product Removed";
            else
                return "Failed to remove item";
        }

        [HttpPut]
        public string UpdateCart(UpdateCartDto cart)
        {
            if (tranRepo.UpdateShoppingCart(cart))
                return "Updated";
            else
                return "Not Updated";
        }

        #endregion

    }
}

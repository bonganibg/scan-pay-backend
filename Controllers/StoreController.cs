using Microsoft.AspNetCore.Mvc;
using ScanPayAPI.Dtos;
using ScanPayAPI.Models;
using ScanPayAPI.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ScanPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {


        private StoreRepository storeRepo = new StoreRepository();
        
        [HttpGet]
        public ActionResult<GetStoreDto> Get([FromQuery] string qr)
        {
            GetStoreDto store = storeRepo.GetStore(qr);        

            if (store == null)
                return NotFound();

            return store;
        }


        [HttpGet("{id}")]
        public IEnumerable<GetStoreDto> GetStores(string id)
        {                        
            return storeRepo.GetBusinessStores(id);
        }

        // Create a new Store 
        [HttpPost]
        public string Post([FromBody] StoreDto store)
        {
            bool result = storeRepo.CreateStoreAccount(store);

            if (result)
                return "It worked";
            else
                return "NO it didnt";
        }

        // Update store information
        [HttpPut]
        public string Put([FromBody] Store store)
        {
            if (storeRepo.UpdateStoreInformation(store))
                return "Done";
            else
                return "Not Done";
        }

        // DELETE api/<StoreController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {

        }
    }
}

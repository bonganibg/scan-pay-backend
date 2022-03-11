using Microsoft.AspNetCore.Cors;
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
    [EnableCors]
    public class StoreController : ControllerBase
    {
        private StoreRepository storeRepo = new StoreRepository();
        
        [HttpGet("{id}")]
        [EnableCors("BusinessApp")]
        public ActionResult<GetStoreDto> Get(string id, [FromQuery] string token)
        {
            if (Authentication.CheckTokenTimeout(token))
            {
                GetStoreDto store = storeRepo.GetStore(id);

                if (store == null)
                    return NotFound();

                return store;
            }
            return null;
        }


        ///Get a list of stores
        [HttpGet]
        [EnableCors("BusinessApp")]
        public ActionResult<Response> GetStores([FromQuery] string token)
        {                        
            if (Authentication.CheckTokenTimeout(token))
            {
                string id = Authentication.GetUserID(token);


                return Ok(new Response()
                {
                    Message = "Stores found",
                    Data = storeRepo.GetBusinessStores(id)
                });
            }

            return NotFound();
        }

        // Create a new Store 
        [HttpPost]
        [EnableCors("BusinessApp")]
        public string Post([FromBody] StoreDto store, [FromQuery] string token)
        {
           if (Authentication.CheckTokenTimeout(token))
            {
                store.BusinessID = Authentication.GetUserID(token).ToString();
                string result = storeRepo.CreateStoreAccount(store);

                if (result.Equals(""))
                    return "Not created";
                else
                    return result;
            }
            return "not created";
        }

        // Update store information
        [HttpPut]
        [EnableCors("BusinessApp")]
        public string Put([FromBody] Store store, [FromQuery] string token)
        {
            if (Authentication.CheckTokenTimeout(token))
            {
                store.BusinessID = Guid.Parse(Authentication.GetUserID(token));
                if (storeRepo.UpdateStoreInformation(store))
                    return "Done";
                else
                    return "Not Done";
            }
            return "Not done";
        }

        // DELETE api/<StoreController>/5
        [HttpDelete("{id}")]
        public void Delete(string id, [FromQuery] string token)
        {
            
        }
    }
}

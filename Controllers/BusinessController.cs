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
    public class BusinessController : ControllerBase
    {

        private BusinessRepository businessRepo = new BusinessRepository();

        [HttpGet]
        public ActionResult<BusinessDto> Get([FromQuery] string id)
        {
            BusinessDto business = businessRepo.GetBusiness(id);

            if (business == null)
                return NotFound();

            return business;
        }


        [HttpPut]
        public ActionResult UpdateBusiness(UpdateBusinessDto business)
        {
            // Check the input using regex

            // Check if the ID exists in the database
            if (businessRepo.GetBusiness(business.BusinessID.ToString()) != null)
            {
                if (businessRepo.UpdateBusiness(business.AsBusiness()))
                    return Accepted();

                return Problem();

            }
            else
                return NotFound();

            // Write the information to the database            

        }

        // Create a business account
        [HttpPost("/create")]
        public ActionResult<string> Post(CreateBusinessDto business)
        {
            bool result = businessRepo.CreateBusinessAccount(business);
            if (result)
                return Ok(result);

            return NotFound();
        }

        // Log into a business account
        [HttpPost("/login")]
        public ActionResult<string> Post(BusinessLoginDto login)
        {
            string userID = businessRepo.CheckLogin(login);

            if (userID == null)
                return NotFound();

            return userID;
        }

    }
}

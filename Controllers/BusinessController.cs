using Microsoft.AspNetCore.Cors;
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
    [EnableCors]
    public class BusinessController : ControllerBase
    {

        private BusinessRepository businessRepo = new BusinessRepository();


        [HttpGet]
        [EnableCors("BusinessApp")]
        public ActionResult<BusinessDto> Get([FromQuery] string token)
        {
            string userID = Authentication.GetUserID(token);

            if (!userID.Equals(""))
            {
                BusinessDto business = businessRepo.GetBusiness(userID);

                if (business == null)
                    return NotFound();

                return business;
            }

            return NotFound();
        }


        [HttpPut]
        [EnableCors("BusinessApp")]
        public ActionResult UpdateBusiness(UpdateBusinessDto business, [FromQuery] string token)
        {
            // Check the input using regex            
            if (Authentication.CheckTokenTimeout(token))
            {
                business.BusinessID = Authentication.GetUserID(token).ToString();
                // Check if the ID exists in the database
                if (businessRepo.GetBusiness(business.BusinessID.ToString()) != null)
                {
                    if (businessRepo.UpdateBusiness(business.AsBusiness()))
                        return Accepted();

                    return Problem();

                }
                else
                    return NotFound();
            }
            return NotFound();

            // Write the information to the database            

        }

        // Create a business account
        [HttpPost("create")]
        [EnableCors("BusinessApp")]
        public IActionResult Post(CreateBusinessDto business)
        {
            string result = businessRepo.CreateBusinessAccount(business);
            if (result.Equals(""))
                return NotFound();

            AuthenticationDto token = new AuthenticationDto()
            {
                Token = Authentication.CreateAuthToken(result, true),
                CreatedAt = DateTime.Now
            };

            return Ok(token);
        }

        // Log into a business account
        [HttpPost("login")]
        [EnableCors("BusinessApp")]
        public ActionResult<AuthenticationDto>  Post(BusinessLoginDto login)
        {
            string userID = businessRepo.CheckLogin(login);

            if (userID == null)
                return NotFound();

            

            AuthenticationDto token = new AuthenticationDto()
            {
                Token = Authentication.CreateAuthToken(userID, true),
                CreatedAt = DateTime.Now
            };

            return Ok(token);
        }

    }
}

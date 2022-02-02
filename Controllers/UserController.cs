using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class UserController : ControllerBase
    {
        private readonly UserRepository userRepo = new UserRepository();

        // Get the Users information
        [HttpGet]
        public ActionResult<GetUserDto> Get([FromQuery] string id)
        {
            GetUserDto getUser = userRepo.GetUser(id);

            if (getUser == null)
                return NotFound();
            else
                return getUser;
        }


        /// <summary>
        /// Get the Login information from the user and check if it is consistant with a record 
        /// in the database
        /// </summary>
        /// <param name="username"> The username or email of the user </param>
        /// <param name="password"> The users password </param>
        /// <returns> If the users information is valid, the User ID will be returned otherwise null will be returned </returns>
        [HttpPost("/login")]
        public ActionResult<string> Post(LoginDto login)
        {
           string response = userRepo.CheckLogin(login).ToString();

            if (response == null)
                return NotFound();
            else
                return response;
        }


        /// <summary>
        /// Get the account information, validate information and create a new account
        /// </summary>
        /// <param name="name">Name of the user</param>
        /// <param name="username">The username used for identifiying and login</param>
        /// <param name="email">Email used for contacting and logging</param>
        /// <param name="phoneNumber">Phone number for contacting the user</param>
        /// <param name="password">Password used to enter the application</param>
        /// <returns>Returns the User ID if the account is created successfully
        /// Returns null if the account creation is not successfull</returns>
        [HttpPost("/create")]
        public ActionResult<User> Post(CreateUserDto userInfo)// can create a custom return DTO
        {
            // Use some regex to validate the user information

            // Check if one of the credentials already exists 

            // Create the account 

            bool result = userRepo.createNewAccount(userInfo);

            if (result)
                return NoContent();
            else
                return NotFound();
        }

        // Update user information
        [HttpPut]
        public ActionResult UpdateUser(UpdateUserDto user)
        {
            // Check the input using regex

            // Check if the ID exists in the database
            if (userRepo.GetUser(user.UserID.ToString()) != null)
            {
                if (userRepo.UpdateAccountInformation(user.AsUser()))
                    return Accepted();

                return Problem();

            }
            else
                return NotFound();

            // Write the information to the database            
                    
        }

        



    }
}

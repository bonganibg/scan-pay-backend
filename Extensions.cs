using ScanPayAPI.Dtos;
using ScanPayAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanPayAPI
{
    public static class Extensions
    {
        public static UserDto AsDto(this User user)
        {
            return new UserDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                Username = user.Username
            };
        }


        public static User AsUser(this UpdateUserDto user)
        {
            return new User
            {
                UserID = Guid.Parse(user.UserID),
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                Username = user.Username
            };
        }

        public static Business AsBusiness(this UpdateBusinessDto business)
        {
            return new Business
            {
                BusinessID = Guid.Parse(business.BusinessID),
                Name = business.Name,
                Email = business.Email,
                PhoneNumber = business.PhoneNumber,
                Password = business.Password,                
            };
        }
    }
}

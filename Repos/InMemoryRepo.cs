using ScanPayAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Repos
{
    public class InMemoryRepo
    {

        private readonly List<User> users = new()
        {
            new User { 
                UserID = Guid.NewGuid(), 
                FullName = "Chase Evans", 
                Username = "ChasingTheEvans", 
                Email = "chasing@gmail.com", 
                PhoneNumber = "0813569874", 
                Password = "WhatThisNeedsAwhat".GetHashCode().ToString() 
            },
            new User
            {
                UserID = Guid.NewGuid(),
                FullName = "Clara Thompson",
                Username = "CarlTheDude",
                Email = "carlg@gmail.com",
                PhoneNumber = "0813569874",
                Password = "WhatIsapassWord".GetHashCode().ToString()
            },
            new User
            {
                UserID = Guid.NewGuid(),
                FullName = "Donald Black",
                Username = "BlackMan",
                Email = "don@gmail.com",
                PhoneNumber = "0813569874",
                Password = "IhastBirds".GetHashCode().ToString()
            },
        };

        public IEnumerable<User> GetUsers()
        {
            return users;
        }

        public User GetUser(Guid id)
        {
            return users.Where(item => item.UserID == id).SingleOrDefault();
        }

        public bool CreateUser(User user)
        {
            users.Add(user);
            return true;
        }

    }
}

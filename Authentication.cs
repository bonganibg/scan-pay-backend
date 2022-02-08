using ScanPayAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI
{
    /// <summary>
    /// When the user logs in, an access token will be created
    /// The access token will prevent client application from sending user IDs to get information
    /// </summary>
    public static class Authentication
    {
        private static Dictionary<Guid, AuthToken> _authenticatedUsers = new Dictionary<Guid, AuthToken>();

        /// Write the users information into the list of valid tokens
        public static string CreateAuthToken(string userID, bool isBusiness)
        {
            AuthToken auth = new AuthToken
            {
                Token = Guid.NewGuid(),
                UserID = Guid.Parse(userID),
                isBusiness = isBusiness,
                Created = DateTime.Now
            };

            _authenticatedUsers.Add(auth.Token, auth);

            return auth.Token.ToString();
        }

        /// Get the user id for an account with a valid token
        public static string GetUserID(string token)
        {
            if (CheckTokenTimeout(token))
                return _authenticatedUsers[Guid.Parse(token)].UserID.ToString();

            return "";
        }


        /// Check if the owner of the token is a business or customer
        public static bool IsBusiness(string token)
        {
            if (CheckTokenTimeout(token))
                return _authenticatedUsers[Guid.Parse(token)].isBusiness;

            return false;
        }

        /// Remove the api key from the list of valid APIs of the token is {24} hours old
        /// If the token is still valid return true
        /// if the token is invalid return false
        public static bool CheckTokenTimeout(string token)
        {
            DateTime currentTime = DateTime.Now;
            AuthToken auth = _authenticatedUsers[Guid.Parse(token)];

            TimeSpan difference = currentTime - auth.Created;

            if (difference.TotalSeconds > 54000)
            {
                _authenticatedUsers.Remove(Guid.Parse(token));
                return false;
            }

            return true;
        }

        ///Remove the users token 
        public static void RemoveToken(string token)
        {
            _authenticatedUsers.Remove(Guid.Parse(token));
        }
    }
}

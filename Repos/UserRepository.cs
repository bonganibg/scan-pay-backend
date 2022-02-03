using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ScanPayAPI.Dtos;
using ScanPayAPI.Models;

namespace ScanPayAPI.Repos
{
    public class UserRepository
    {
        SqlConnection _conn = new SqlConnection();

        private void Connection()
        {
            string connectionString = Database.getConnectionString();
            _conn = new SqlConnection(connectionString);
        }


        #region Create Account

        /// <summary>
        /// Generate the passphrase which will be used to rest the users account
        /// </summary>
        /// <returns>Passphrase used to reset the account</returns>
        private string generatePassPhrase()
        {
            return "There be pirates on these seas";
        }


        /// <summary>
        /// Get the entered user information and add the missing information for the full model
        /// </summary>
        /// <param name="userInfo"> DTO with the entered information </param>
        /// <returns>User model with a Guid </returns>
        private User createUserAccount(CreateUserDto userInfo)
        {
            return new User()
            {
                UserID = Guid.NewGuid(),
                Email = userInfo.Email,
                FullName = userInfo.FullName,
                Username = userInfo.Username,
                PhoneNumber = userInfo.PhoneNumber,
                Password = userInfo.Password.GetHashCode().ToString()
            };
        }

        public bool createNewAccount(CreateUserDto userInfo)
        {
            User user = createUserAccount(userInfo);

            Connection();
            SqlCommand createUser = new SqlCommand("manageUserAccount", _conn);
            createUser.CommandType = CommandType.StoredProcedure;
            createUser.Parameters.AddWithValue("@UserID", user.UserID.ToString());
            createUser.Parameters.AddWithValue("@Name", user.FullName);
            createUser.Parameters.AddWithValue("@Username", user.Username);
            createUser.Parameters.AddWithValue("@Email", user.Email);
            createUser.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            createUser.Parameters.AddWithValue("@Password", user.Password);
            createUser.Parameters.AddWithValue("@PassPhrase", generatePassPhrase());

            _conn.Open();
            int i = createUser.ExecuteNonQuery();
            _conn.Close();

            //EnterBankingInformation(user.UserID.ToString());

            if (i >= 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Set the default values for the users banking and billing information
        /// </summary>
        /// <param name="userId"></param>
        private void EnterBankingInformation(string userId)
        {
            BankingRepository bankingRepo = new();
            CreateBankingDto banking = new()
            {
                CardName = "",
                CardNumber = "",
                CVV = "",
                ExpiryDate = "",
                holderID = userId,
                isBusiness = false

            };

            CreateBillingDto billing = new()
            {
                holderID = userId,
                isBusiness = false,
                AddressOne = "",
                AddressTwo = "",
                City = "",
                Country = "",
                State = "",
                Zip = ""
            };

            bankingRepo.CreateBankingInformation(banking);
            bankingRepo.EnterBillingAddress(billing);
        }

        #endregion

        #region Login To Account
        
        public string CheckLogin(LoginDto loginInfo)
        {
            loginInfo.Password = loginInfo.Password.GetHashCode().ToString();

            Connection();
            SqlCommand checkLogin = new SqlCommand("userLogin", _conn);
            checkLogin.CommandType = CommandType.StoredProcedure;
            checkLogin.Parameters.AddWithValue("@Email", loginInfo.Email);
            checkLogin.Parameters.AddWithValue("@Password", loginInfo.Password);

            SqlDataAdapter da = new SqlDataAdapter(checkLogin);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            string userInfo = loginInfo.Password;

            foreach(DataRow dr in dt.Rows)
            {
                userInfo = Convert.ToString(dr["user_id"]);
            }

            return userInfo;
        }

        #endregion

        #region Get Account Information

        public GetUserDto GetUser(string userID)
        {            
            Connection();
            SqlCommand getUser = new SqlCommand("getUserInfo", _conn);
            getUser.CommandType = CommandType.StoredProcedure;
            getUser.Parameters.AddWithValue("@UserID", userID);

            SqlDataAdapter da = new SqlDataAdapter(getUser);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            foreach(DataRow dr in dt.Rows)
            {
                return new GetUserDto
                {
                    FullName = Convert.ToString(dr["full_name"]),
                    Username = Convert.ToString(dr["username"]),
                    Email = Convert.ToString(dr["email"]),
                    PhoneNumber = Convert.ToString(dr["phone_number"])
                };
            }

            return null;
        }



        #endregion

        #region Update Account Information
        
        public bool UpdateAccountInformation(UpdateUserDto user)
        {
            Connection();
            SqlCommand updateUser = new SqlCommand("updateUserInfo", _conn);
            updateUser.CommandType = CommandType.StoredProcedure;
            updateUser.Parameters.AddWithValue("@UserID", user.UserID);
            updateUser.Parameters.AddWithValue("@Name", user.FullName);
            updateUser.Parameters.AddWithValue("@Username", user.Username);
            updateUser.Parameters.AddWithValue("@Email", user.Email);
            updateUser.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
            updateUser.Parameters.AddWithValue("@Password", user.Password.GetHashCode().ToString());

            _conn.Open();
            int i = updateUser.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return true;
            
            return false;
        }

        #endregion
    }
}

﻿using ScanPayAPI.Dtos;
using ScanPayAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Repos
{
    public class BusinessRepository
    {
        SqlConnection _conn = new SqlConnection();

        private void Connection()
        {
            string connectionString = Database.getConnectionString();
            _conn = new SqlConnection(connectionString);
        }


        #region Create New Business
        
        private string generatePassPhrase()
        {
            return "There be pirates on these seas";
        }


        private Business WriteBusinessInfo(CreateBusinessDto business)
        {
            return new Business()
            {
                BusinessID = Guid.NewGuid(),
                Email = business.Email,
                Name = business.Name,               
                PhoneNumber = business.PhoneNumber,
                Password = business.Password.GetHashCode().ToString()                
            };
        }

        public bool CreateBusinessAccount(CreateBusinessDto business)
        {
            Business bus = WriteBusinessInfo(business);

            Connection();
            SqlCommand createBusiness = new SqlCommand("manageBusinessAccount", _conn);
            createBusiness.CommandType = CommandType.StoredProcedure;
            createBusiness.Parameters.AddWithValue("@BusinessID", bus.BusinessID.ToString());
            createBusiness.Parameters.AddWithValue("@Name", bus.Name);
            createBusiness.Parameters.AddWithValue("@Email", bus.Email);
            createBusiness.Parameters.AddWithValue("@PhoneNumber", bus.PhoneNumber);
            createBusiness.Parameters.AddWithValue("@Password", bus.Password);
            createBusiness.Parameters.AddWithValue("@PassPhrase", generatePassPhrase());

            _conn.Open();
            int i = createBusiness.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        #endregion

        #region Login To Business

        public string CheckLogin(BusinessLoginDto login)
        {

            Connection();
            SqlCommand businessLogin = new SqlCommand("businessLogin", _conn);
            businessLogin.CommandType = CommandType.StoredProcedure;
            businessLogin.Parameters.AddWithValue("@Email", login.Email);
            businessLogin.Parameters.AddWithValue("@Password", login.Password.GetHashCode().ToString());
            SqlDataAdapter da = new SqlDataAdapter(businessLogin);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            foreach(DataRow dr in dt.Rows)
            {
                return Convert.ToString(dr["business_id"]);
            }

            return null;

        }


        #endregion

        #region Get Business 

        public BusinessDto GetBusiness(string businessID)
        {
            Connection();
            SqlCommand getBusiness = new SqlCommand("getBusinessInfo", _conn);
            getBusiness.CommandType = CommandType.StoredProcedure;
            getBusiness.Parameters.AddWithValue("@BusinessID", businessID);

            SqlDataAdapter da = new SqlDataAdapter(getBusiness);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            foreach (DataRow dr in dt.Rows)
            {
                return new BusinessDto
                {
                    Name = Convert.ToString(dr["name"]),
                    Email = Convert.ToString(dr["email"]),
                    PhoneNumber = Convert.ToString(dr["phone_number"])
                };
            }

            return null;
        }

        #endregion

        #region Update Business Info
            
        public bool UpdateBusiness(Business business)
        {
            Connection();
            SqlCommand updateBusiness = new SqlCommand("updateBusinessInfo");
            updateBusiness.CommandType = CommandType.StoredProcedure;
            updateBusiness.Parameters.AddWithValue("@BusinessID", business.BusinessID);
            updateBusiness.Parameters.AddWithValue("@Name", business.Name);            
            updateBusiness.Parameters.AddWithValue("@Email", business.Email);
            updateBusiness.Parameters.AddWithValue("@PhoneNumber", business.PhoneNumber);
            updateBusiness.Parameters.AddWithValue("@Password", business.Password.GetHashCode().ToString());

            _conn.Open();
            int i = updateBusiness.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return true;

            return false;
        }

        #endregion
    }
}
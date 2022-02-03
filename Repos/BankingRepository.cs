using ScanPayAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Repos
{
    public class BankingRepository
    {
        SqlConnection _conn = new SqlConnection();

        private void Connection()
        {
            string connectionString = Database.getConnectionString();
            _conn = new SqlConnection(connectionString);
        }

        #region Create Banking and Billing Address

        
        /// Enter the banking information and link it to the user 
        public bool CreateBankingInformation(CreateBankingDto banking)
        {
            Connection();

            SqlCommand enterBanking;            

            if (banking.isBusiness)
                enterBanking = new SqlCommand("enterBankingInfoBusiness", _conn);
            else
                enterBanking = new SqlCommand("enterBankingInfoUser", _conn);


            enterBanking.CommandType = CommandType.StoredProcedure;

            if (banking.isBusiness)
                enterBanking.Parameters.AddWithValue("@BusinessID", banking.holderID);
            else
                enterBanking.Parameters.AddWithValue("@UserID", banking.holderID);
            

            enterBanking.Parameters.AddWithValue("@CardID", Guid.NewGuid().ToString());
            enterBanking.Parameters.AddWithValue("@CardNumber", banking.CardNumber);
            enterBanking.Parameters.AddWithValue("@ExpiryDate", banking.ExpiryDate);
            enterBanking.Parameters.AddWithValue("@CVV", banking.CVV);
            enterBanking.Parameters.AddWithValue("@NameOfCard", banking.CardName);

            _conn.Open();
            int i = enterBanking.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        ///Enter the billing address infromation
        public bool EnterBillingAddress(CreateBillingDto billing)
        {
            Connection();
            SqlCommand enterBilling;


            enterBilling = new SqlCommand("enterBillingInformationBusiness", _conn);
            enterBilling.CommandType = CommandType.StoredProcedure;

            if (billing.isBusiness)
                enterBilling = new SqlCommand("enterBillingInformationBusiness", _conn);
            else
                enterBilling = new SqlCommand("enterBillingInformationUser", _conn);

            enterBilling.CommandType = CommandType.StoredProcedure;
            if (billing.isBusiness)
            {
                enterBilling.Parameters.AddWithValue("@BusinessID", billing.holderID);
                enterBilling.Parameters.AddWithValue("@BusinessBillingID", Guid.NewGuid().ToString());
            }
            else
            {
                enterBilling.Parameters.AddWithValue("@UserBillingID", billing.holderID);
                enterBilling.Parameters.AddWithValue("@UserBillingID", Guid.NewGuid().ToString());
            }                     
            
            enterBilling.Parameters.AddWithValue("@BillingID", Guid.NewGuid().ToString());
            enterBilling.Parameters.AddWithValue("@Country", billing.Country);
            enterBilling.Parameters.AddWithValue("@AddressLineOne", billing.AddressOne);
            enterBilling.Parameters.AddWithValue("@AddressLineTwo", billing.AddressTwo);
            enterBilling.Parameters.AddWithValue("@City", billing.City);
            enterBilling.Parameters.AddWithValue("@Zip", billing.Zip);
            enterBilling.Parameters.AddWithValue("@State", billing.State);

            _conn.Open();
            int i = enterBilling.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        #endregion



    }
}

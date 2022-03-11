using ScanPayAPI.Dtos;
using ScanPayAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Repos
{
    public class StoreRepository
    {
        SqlConnection _conn = new SqlConnection();

        private void Connection()
        {
            string connectionString = Database.getConnectionString();
            _conn = new SqlConnection(connectionString);
        }

        #region Create Store

        public string CreateStoreAccount(StoreDto storeInfo)
        {
            

            Store store = new Store()
            {
                BusinessID = Guid.Parse(storeInfo.BusinessID),
                Name = storeInfo.Name,
                QrCode = "a5sdas4d6a4d64asd",
                StoreId = Guid.NewGuid(),
                ImageUri = storeInfo.ImageUri
            };

            Connection();
            SqlCommand createStore = new SqlCommand("manageStoreInformation", _conn);
            createStore.CommandType = CommandType.StoredProcedure;
            createStore.Parameters.AddWithValue("@StoreID", store.StoreId.ToString());
            createStore.Parameters.AddWithValue("@BusinessID", store.BusinessID.ToString());
            createStore.Parameters.AddWithValue("@Name", store.Name);
            createStore.Parameters.AddWithValue("@QrCode", store.QrCode);
            createStore.Parameters.AddWithValue("@ImageUri", store.ImageUri);


            _conn.Open();
            int i = createStore.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return store.StoreId.ToString();
            else
                return "";
        }


        #endregion

        #region Get Stores

        // Get Single Store 
        public GetStoreDto GetStore(string storeID)
        {
            Connection();
            SqlCommand getStore = new SqlCommand("getStoreDetails", _conn);
            getStore.CommandType = CommandType.StoredProcedure;
            getStore.Parameters.AddWithValue("@StoreID", storeID);

            SqlDataAdapter da = new SqlDataAdapter(getStore);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            foreach (DataRow dr in dt.Rows)
            {
                return new GetStoreDto
                {
                    StoreId = Convert.ToString(dr["store_id"]),
                    Name = Convert.ToString(dr["name"]),
                    QrCode = Convert.ToString(dr["qr_code"]),
                    ImageUri = Convert.ToString(dr["image_uri"])
                };
            }

            return null;
        }


        // Get stores from business ID
        public IEnumerable<StoreTrendsDto> GetBusinessStores(string businessID)
        {
            Connection();
            SqlCommand getStore = new SqlCommand("getBusinessStores", _conn);
            getStore.CommandType = CommandType.StoredProcedure;
            getStore.Parameters.AddWithValue("@BusinessID", businessID);

            SqlDataAdapter da = new SqlDataAdapter(getStore);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            List<StoreTrendsDto> stores = new();

            foreach(DataRow dr in dt.Rows)
            {
                stores.Add(new StoreTrendsDto
                {
                    Name = Convert.ToString(dr["name"]),                    
                    StoreID = Convert.ToString(dr["store_id"]),
                    ImageUri = Convert.ToString(dr["image_uri"]),
                    Trend = Convert.ToDecimal(-0.53),
                    Sales = Convert.ToInt32(13)
                });
            }

            return stores;
        }      


        #endregion

        #region Update Store Information
        
        public bool UpdateStoreInformation(Store store)
        {
            Connection();
            SqlCommand updateStore = new SqlCommand("updateStoreInformation", _conn);
            updateStore.CommandType = CommandType.StoredProcedure;
            updateStore.Parameters.AddWithValue("@StoreID", store.StoreId);
            updateStore.Parameters.AddWithValue("@BusinessID", store.BusinessID);
            updateStore.Parameters.AddWithValue("@Name", store.Name);
            updateStore.Parameters.AddWithValue("@QrCode", store.QrCode);
            updateStore.Parameters.AddWithValue("@ImageUri", store.ImageUri);

            _conn.Open();
            int i = updateStore.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return true;

            return false;
        }

        #endregion
    }
}

using ScanPayAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ScanPayAPI.Repos
{
    public class TransactionRepository
    {
        SqlConnection _conn = new SqlConnection();

        private void Connection()
        {
            string connectionString = Database.getConnectionString();
            _conn = new SqlConnection(connectionString);
        }


        #region Check If there is cart

        ///Check if a cart exists 
        public string CheckIfCartExists(string userID, string storeID)
        {
            Connection();
            SqlCommand checkCart = new SqlCommand("checkCart", _conn);
            checkCart.CommandType = CommandType.StoredProcedure;
            checkCart.Parameters.AddWithValue("@UserID", userID);
            checkCart.Parameters.AddWithValue("@StoreID", storeID);

            SqlDataAdapter da = new SqlDataAdapter(checkCart);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            foreach (DataRow dr in dt.Rows)
            {
                return Convert.ToString(dr["cart_id"]);
            }

            return "";
        }

        #endregion

        #region Create Shopping cart

        /// Create a cart shopping cart for the user which will link
        public string CreateShoppingCart(CartDto cartInfo)
        {            
            string checkCart = CheckIfCartExists(cartInfo.CustomerID, cartInfo.StoreID);

            // Check if there is a cart already to avoid dublicates
            if (!checkCart.Equals(checkCart))
            {
                Connection();

                string cartID = Guid.NewGuid().ToString();

                SqlCommand addToCart = new SqlCommand("addToShoppingCart", _conn);
                addToCart.CommandType = CommandType.StoredProcedure;
                addToCart.Parameters.AddWithValue("@CartID", cartID);
                addToCart.Parameters.AddWithValue("@UserID", cartInfo.CustomerID);
                addToCart.Parameters.AddWithValue("@StoreID", cartInfo.StoreID);

                _conn.Open();
                int i = addToCart.ExecuteNonQuery();
                _conn.Close();

                if (i >= 1)
                    return cartID;
                else
                    return "";
            }
            else
                return checkCart;
        }

        /// Add the users product to the database table which links to their cart
        public bool AddProductToCart(CartDto cartItem)
        {
            string cartID = CreateShoppingCart(cartItem);

            if (!cartID.Equals(""))
            {
                Connection();

                SqlCommand addToCart = new SqlCommand("addProductToCart", _conn);
                addToCart.CommandType = CommandType.StoredProcedure;
                addToCart.Parameters.AddWithValue("@CartID", cartID);
                addToCart.Parameters.AddWithValue("@ProductID", cartItem.ProductID);
                addToCart.Parameters.AddWithValue("@StoreID", cartItem.StoreID);
                addToCart.Parameters.AddWithValue("@Quantity", cartItem.Quantity);

                _conn.Open();
                int i = addToCart.ExecuteNonQuery();
                _conn.Close();

                if (i >= 1)
                    return true;                
            }

            return false;
        }

        #endregion

        #region Remove Item from cart

        public bool RemoveItemFromCart(RemoveItemDto remove)
        {
            Connection();
            SqlCommand removeItem = new SqlCommand("removeCartItem", _conn);
            removeItem.CommandType = CommandType.StoredProcedure;
            removeItem.Parameters.AddWithValue("@CartID", remove.CartID);
            removeItem.Parameters.AddWithValue("@ProductID", remove.ProductID);

            _conn.Open();
            int i = removeItem.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        #endregion

        #region Update Shopping cart

        ///Update the product information from the shopping cart
        public bool UpdateShoppingCart(UpdateCartDto cart)
        {
            Connection();

            SqlCommand updateCart = new SqlCommand("updateCartProduct", _conn);
            updateCart.CommandType = CommandType.StoredProcedure;
            updateCart.Parameters.AddWithValue("@CartID", cart.CartID);
            updateCart.Parameters.AddWithValue("@Quantity", cart.Quantity);

            _conn.Open();
            int i = updateCart.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        #endregion


    }
}

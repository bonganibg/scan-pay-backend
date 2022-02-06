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
    public class TransactionRepository
    {
        SqlConnection _conn = new SqlConnection();

        private void Connection()
        {
            string connectionString = Database.getConnectionString();
            _conn = new SqlConnection(connectionString);
        }



        #region Cart Functions 


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

        #region Get Shopping Cart Items

        public List<Product> GetCartItems(string cartID)
        {
            Connection();
            SqlCommand getCart = new SqlCommand("getCartItems", _conn);
            getCart.CommandType = CommandType.StoredProcedure;
            getCart.Parameters.AddWithValue("@CartID", cartID);

            SqlDataAdapter da = new SqlDataAdapter(getCart);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();


            List<Product> products = new();

            foreach (DataRow dr in dt.Rows)
            {
                products.Add(new Product 
                { 
                    ProductID = Guid.Parse(Convert.ToString(dr["product_id"])),    
                    Barcodes = Convert.ToString(dr["qr_code"]),
                    Name = Convert.ToString(dr["name"]),
                    Price = Convert.ToDecimal(dr["price"]),
                    Description = Convert.ToString(dr["description"]),
                    SalePrice = Convert.ToDecimal(dr["sale_price"]),
                    Sale = Convert.ToBoolean(dr["sale"]),
                    Stock = Convert.ToInt32(dr["quantity"])
                });
            }

            return products;

        }

        #endregion

        #endregion


        #region Checkout functions      

        ///IF the user has not entered their banking information, they should be prompted to 
        ///Version two will allow the user to use a card once off without it having to be in the DB
        public Receipt Checkout(CheckoutDto checkout)
        {            
            string cardID = CheckBankingDetails(checkout.CustomerID);
            string receiptID = Guid.NewGuid().ToString();

            if (!cardID.Equals(""))
            {                               
                List<ReceiptProductDto> products = MigrateCartToReceipt(checkout.CustomerID, checkout.StoreID, receiptID);
                if (products.Count > 0)
                {

                    decimal total = products.Sum(prod => prod.TotalPrice);

                    // Take the money from the account over here, if successful, print the receipt                    
                    if (1==1)
                    {
                        Connection();
                        SqlCommand writeReceipt = new SqlCommand("createReceipt", _conn);
                        writeReceipt.Parameters.AddWithValue("@ReceiptID", receiptID);
                        writeReceipt.Parameters.AddWithValue("@StoreID", checkout.StoreID);
                        writeReceipt.Parameters.AddWithValue("@UserID", checkout.CustomerID);                                                ;
                        writeReceipt.Parameters.AddWithValue("@QrCode", GenerateCode.QrCode());
                        writeReceipt.Parameters.AddWithValue("@TotalPrice", total);
                        writeReceipt.Parameters.AddWithValue("@BillingDate", new DateTime().ToShortDateString());

                        _conn.Open();
                        int i = writeReceipt.ExecuteNonQuery();
                        _conn.Close();
                    }
                }
            }

            return GetReceipt(receiptID);
        }

        ///Get Receipt
        private Receipt GetReceipt(string receiptID)
        {
            Connection();
            SqlCommand getReceipt = new SqlCommand("getReceipt", _conn);
            getReceipt.CommandType = CommandType.StoredProcedure;
            getReceipt.Parameters.AddWithValue("@ReceiptID", receiptID);

            SqlDataAdapter da = new SqlDataAdapter(getReceipt);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            Receipt receipt = null;

            foreach(DataRow dr in dt.Rows)
            {
                return new Receipt()
                {
                    ReceiptID = Guid.Parse(Convert.ToString(dr["receipt_id"])),
                    StoreID = Guid.Parse(Convert.ToString(dr["store_id"])),
                    UserID = Guid.Parse(Convert.ToString(dr["user_id"])),
                    QrCode = Convert.ToString(dr["qr_code"]),
                    TotalPrice = Convert.ToDecimal(dr["total_price"]),
                    Date = Convert.ToDateTime(dr["billing_date"])                    
                };
            }

            return receipt;
        }

        /// Write the products from the Cart to the receipt and get the totals for the product
        private List<ReceiptProductDto> MigrateCartToReceipt(string userID, string storeID, string receiptID)
        {
            List<ReceiptProductDto> products = WriteProductsToReceipt(CheckIfCartExists(userID, storeID));

            if (products.Count > 0)
            {
                Connection();
                SqlCommand writeToReceipt = new SqlCommand("receiptProducts", _conn);
                writeToReceipt.CommandType = CommandType.StoredProcedure;

                foreach(ReceiptProductDto prod in products)
                {                    
                    if (prod.Sale)
                        prod.TotalPrice = prod.SalePrice * prod.Quantity;
                    else
                        prod.TotalPrice = prod.Price * prod.Quantity;


                    writeToReceipt.Parameters.AddWithValue("@ReceiptID", receiptID);
                    writeToReceipt.Parameters.AddWithValue("@ProductID", prod.ProductID);
                    
                    if (prod.Sale)
                        writeToReceipt.Parameters.AddWithValue("@Price", prod.SalePrice);
                    else
                        writeToReceipt.Parameters.AddWithValue("@Price", prod.Price);

                    writeToReceipt.Parameters.AddWithValue("@Quantity", prod.Quantity);
                    writeToReceipt.Parameters.AddWithValue("@Total", prod.TotalPrice);


                    _conn.Open();
                    int i = writeToReceipt.ExecuteNonQuery();
                    _conn.Close();                    
                }                
            }

            return products;

        }

        /// Get items from the shopping cart
        private List<ReceiptProductDto> WriteProductsToReceipt(string cardID)
        {
            Connection();
            SqlCommand getReceiptItems = new SqlCommand("receiptProducts", _conn);
            getReceiptItems.CommandType = CommandType.StoredProcedure;
            getReceiptItems.Parameters.AddWithValue("@CartID",cardID);

            SqlDataAdapter da = new SqlDataAdapter(getReceiptItems);
            DataTable dt = new DataTable();

            _conn.Close();
            da.Fill(dt);
            _conn.Close();

            List<ReceiptProductDto> items = new();

            foreach (DataRow dr in dt.Rows)
            {
                items.Add(new ReceiptProductDto 
                { 
                    ProductID = Convert.ToString(dr["product_id"]),
                    Name = Convert.ToString(dr["name"]),    
                    Price = Convert.ToDecimal(dr["price"]),    
                    Quantity = Convert.ToInt32(dr["quantity"]),    
                    Sale = Convert.ToBoolean(dr["sale"]),    
                    SalePrice = Convert.ToDecimal(dr["sale_price"])
                });
            }

            return items;       
            
        }


        /// Check if the user has entered their banking information
        /// if not return an empty string 
        /// If so return a string with the card id
        private string CheckBankingDetails(string userID)
        {
            Connection();
            SqlCommand checkBanking = new SqlCommand("clientBanking", _conn);
            checkBanking.CommandType = CommandType.StoredProcedure;
            checkBanking.Parameters.AddWithValue("@UserID", userID);

            SqlDataAdapter da = new SqlDataAdapter(checkBanking);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            if (dt.Rows.Count > 0)
                return dt.Rows[0]["card_id"].ToString();
            else
                return "";

        }

        

        #endregion

    }
}

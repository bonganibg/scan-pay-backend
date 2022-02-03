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
    public class ProductRepository
    {
        SqlConnection _conn = new SqlConnection();        

        private void Connection()
        {
            string connectionString = Database.getConnectionString();
            _conn = new SqlConnection(connectionString);
        }

        #region Create Product

        ///Create a new product 
        public bool CreateNewProduct(CreateProductDto product)
        {

            string productID = Guid.NewGuid().ToString();
            Connection();
            SqlCommand createProduct = new SqlCommand("manageProduct", _conn);
            createProduct.CommandType = CommandType.StoredProcedure;
            createProduct.Parameters.AddWithValue("@ProductID", productID);
            createProduct.Parameters.AddWithValue("@QrCode", product.Barcode);
            createProduct.Parameters.AddWithValue("@Name", product.Name);
            createProduct.Parameters.AddWithValue("@Price", product.Price);
            createProduct.Parameters.AddWithValue("@Description", product.Description);
            createProduct.Parameters.AddWithValue("@ImageUri", product.ImageUri);
            createProduct.Parameters.AddWithValue("@SalePrice", product.SalePrice);
            createProduct.Parameters.AddWithValue("@Sale", product.Sale);
            createProduct.Parameters.AddWithValue("@Stock", product.Stock);

            _conn.Open();
            int i = createProduct.ExecuteNonQuery();
            _conn.Close();

            WriteProductsToStore("", "");

            if (i >= 1)
                return true;
            else
                return false;

        }

        private void WriteProductsToStore(string storeID, string productID)
        {
            Connection();
            SqlCommand linkProduct = new SqlCommand("writeProductsToStore", _conn);
            linkProduct.CommandType = CommandType.StoredProcedure;
            linkProduct.Parameters.AddWithValue("@StoreID", "247a4d8e-0fe0-4cd8-8a30-2d19049e8017");
            linkProduct.Parameters.AddWithValue("@ProductID", "6eff4a2b-f1b9-4a4e-9831-444547f7e54");

            _conn.Open();
            int i = linkProduct.ExecuteNonQuery();
            _conn.Close();
        }

        #endregion

        #region Get product information

        public Product GetProduct(string barcode)
        {
            Connection();
            SqlCommand getProduct = new SqlCommand("getProductInformation", _conn);
            getProduct.CommandType = CommandType.StoredProcedure;
            getProduct.Parameters.AddWithValue("@QrCode", barcode);

            SqlDataAdapter da = new SqlDataAdapter(getProduct);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();


            foreach(DataRow dr in dt.Rows)
                    {
                return new Product()
                {
                    ProductID = Guid.Parse(Convert.ToString(dr["product_id"])),
                    Barcodes = barcode,
                    Name = Convert.ToString(dr["name"]),
                    Description = Convert.ToString(dr["description"]),
                    ImageUri = Convert.ToString(dr["image_uri"]),
                    Sale = Convert.ToBoolean(dr["sale"]),
                    SalePrice = Convert.ToDecimal(dr["sale_price"]),
                    Price = Convert.ToDecimal(dr["price"]),
                    Stock = Convert.ToInt32(dr["stock"])
                };
            }

            return null;
        }

        #endregion



        #region Add Product to basket



        #endregion

    }
}

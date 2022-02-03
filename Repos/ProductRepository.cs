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

            WriteProductsToStore(product.StoreID, productID);

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
            linkProduct.Parameters.AddWithValue("@ProductID", productID);
            linkProduct.Parameters.AddWithValue("@StoreID", storeID);


            _conn.Open();
            int i = linkProduct.ExecuteNonQuery();
            _conn.Close();
        }

        #endregion

        #region Get product information

        /// Get a product from the barcode 
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


            foreach (DataRow dr in dt.Rows)
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


        ///Get the products that are available in a store
        public List<Product> GetProducts(string storeID)
        {
            Connection();
            SqlCommand getProducts = new SqlCommand("getStoreProducts", _conn);
            getProducts.CommandType = CommandType.StoredProcedure;
            getProducts.Parameters.AddWithValue("@StoreID", storeID);

            SqlDataAdapter da = new SqlDataAdapter(getProducts);
            DataTable dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            List<Product> products = new();

            foreach (DataRow dr in dt.Rows)
            {
                Product prod = new Product
                {
                    ProductID = Guid.Parse(Convert.ToString(dr["product_id"])),
                    Barcodes = Convert.ToString(dr["qr_code"]),
                    Name = Convert.ToString(dr["name"]),
                    Price = Convert.ToDecimal(dr["price"]),
                    Description = Convert.ToString(dr["description"]),
                    ImageUri = Convert.ToString(dr["image_uri"]),
                    SalePrice = Convert.ToDecimal(dr["sale_price"]),
                    Sale = Convert.ToBoolean(dr["sale"]),
                    Stock = Convert.ToInt32(dr["stock"])
                };

                products.Add(prod);
            }

            return products;

        }

        #endregion

        #region Update Product Information

        public bool UpdateProduct(Product product)
        {
            Connection();

            SqlCommand updateProd = new SqlCommand("updateProduct", _conn);
            updateProd.CommandType = CommandType.StoredProcedure;
            updateProd.Parameters.AddWithValue("@ProductID", product.ProductID);
            updateProd.Parameters.AddWithValue("@QrCode", product.Barcodes);
            updateProd.Parameters.AddWithValue("@Name", product.Name);
            updateProd.Parameters.AddWithValue("@Price", product.Price);
            updateProd.Parameters.AddWithValue("@Description", product.Description);
            updateProd.Parameters.AddWithValue("@ImageUri", product.ImageUri);
            updateProd.Parameters.AddWithValue("@SalePrice", product.SalePrice);
            updateProd.Parameters.AddWithValue("@Sale", product.Sale);
            updateProd.Parameters.AddWithValue("@Stock", product.Stock);

            _conn.Open();
            int i = updateProd.ExecuteNonQuery();
            _conn.Close();

            if (i >= 1)
                return true;
            else
                return false;

        }


        #endregion


    }
}

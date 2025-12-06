using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using MarketStockTracking.Models;

namespace MarketStockTracking.Repositories
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public SqlProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Product> GetAll()
        {
            var products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var query = "SELECT ProductID, ProductName, ProductType, AddedDate FROM Products ORDER BY AddedDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        products.Add(new Product
                        {
                            ProductID = rdr.GetInt32(0),
                            ProductName = rdr.GetString(1),
                            ProductType = rdr.GetString(2),
                            AddedDate = rdr.GetDateTime(3)
                        });
                    }
                }
            }

            return products;
        }

        public int Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery();
            }
        }

        public int Add(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var query = @"INSERT INTO Products (ProductName, ProductType, AddedDate) 
                              VALUES (@name, @type, @date)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", product.ProductName);
                    cmd.Parameters.AddWithValue("@type", product.ProductType);
                    cmd.Parameters.AddWithValue("@date", product.AddedDate);

                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

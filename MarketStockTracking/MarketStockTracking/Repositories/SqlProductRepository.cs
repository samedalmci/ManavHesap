using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
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
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT ProductID, ProductName, ProductType, AddedDate FROM Products ORDER BY AddedDate DESC";
                using (var cmd = new SqliteCommand(query, conn))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        products.Add(new Product
                        {
                            ProductID = rdr.GetInt32(0),
                            ProductName = rdr.GetString(1),
                            ProductType = rdr.GetString(2),
                            AddedDate = DateTime.Parse(rdr.GetString(3))
                        });
                    }
                }
            }
            return products;
        }

        public int Delete(int id)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqliteCommand("DELETE FROM Products WHERE ProductID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery();
            }
        }

        public int Add(Product product)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                var query = @"INSERT INTO Products (ProductName, ProductType, AddedDate) 
                              VALUES (@name, @type, @date)";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", product.ProductName);
                    cmd.Parameters.AddWithValue("@type", product.ProductType);
                    cmd.Parameters.AddWithValue("@date", product.AddedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
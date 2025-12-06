using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using MarketStockTracking.Models;

namespace MarketStockTracking.Repositories
{
    public class SqlSalesRepository : ISalesRepository
    {
        private readonly string _connectionString;

        public SqlSalesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Sale> GetAll()
        {
            var sales = new List<Sale>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Sales ORDER BY ID DESC";
                using (var cmd = new SqliteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sales.Add(new Sale
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ProductName = reader["ProductName"].ToString(),
                            StoreName = reader["StoreName"].ToString(),
                            Quantity = Convert.ToDecimal(reader["Quantity"]),
                            NetPrice = Convert.ToDecimal(reader["NetPrice"]),
                            GrossPrice = Convert.ToDecimal(reader["GrossPrice"]),
                            Profit = Convert.ToDecimal(reader["Profit"]),
                            CashPaid = Convert.ToDecimal(reader["CashPaid"]),
                            Debt = Convert.ToDecimal(reader["Debt"]),
                            CreatedDate = DateTime.Parse(reader["CreatedDate"].ToString())
                        });
                    }
                }
            }
            return sales;
        }

        public int Add(Sale sale)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO Sales
                    (ProductName, StoreName, Quantity, NetPrice, GrossPrice, Profit, CashPaid, Debt, CreatedDate)
                    VALUES
                    (@ProductName, @StoreName, @Quantity, @NetPrice, @GrossPrice, @Profit, @CashPaid, @Debt, @CreatedDate)
                ";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductName", sale.ProductName);
                    cmd.Parameters.AddWithValue("@StoreName", sale.StoreName);
                    cmd.Parameters.AddWithValue("@Quantity", sale.Quantity);
                    cmd.Parameters.AddWithValue("@NetPrice", sale.NetPrice);
                    cmd.Parameters.AddWithValue("@GrossPrice", sale.GrossPrice);
                    cmd.Parameters.AddWithValue("@Profit", sale.Profit);
                    cmd.Parameters.AddWithValue("@CashPaid", sale.CashPaid);
                    cmd.Parameters.AddWithValue("@Debt", sale.Debt);
                    cmd.Parameters.AddWithValue("@CreatedDate", sale.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
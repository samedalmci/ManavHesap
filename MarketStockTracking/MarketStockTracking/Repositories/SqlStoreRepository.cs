using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using MarketStockTracking.Models;

namespace MarketStockTracking.Repositories
{
    public class SqlStoreRepository : IStoreRepository
    {
        private readonly string _connectionString;

        public SqlStoreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Store> GetAll()
        {
            var stores = new List<Store>();
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                var query = "SELECT StoreID, StoreName, StoreDate FROM Stores ORDER BY StoreDate DESC";
                using (var cmd = new SqliteCommand(query, conn))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        stores.Add(new Store
                        {
                            StoreID = rdr.GetInt32(0),
                            StoreName = rdr.GetString(1),
                            StoreDate = DateTime.Parse(rdr.GetString(2))
                        });
                    }
                }
            }
            return stores;
        }

        public int Delete(int id)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqliteCommand("DELETE FROM Stores WHERE StoreID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery();
            }
        }

        public int Add(Store store)
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();
                var query = @"INSERT INTO Stores (StoreName, StoreDate) 
                              VALUES (@name, @date)";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", store.StoreName);
                    cmd.Parameters.AddWithValue("@date", store.StoreDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
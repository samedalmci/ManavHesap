using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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

            using (SqlConnection conn = new SqlConnection(_connectionString)) 
            { 
                conn.Open();
                var query = "SELECT StoreID, StoreName, StoreDate FROM Stores ORDER BY StoreDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn)) 
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        stores.Add(new Store
                        {
                            StoreID = rdr.GetInt32(0),
                            StoreName = rdr.GetString(1),
                            StoreDate = rdr.GetDateTime(2)
                        });
                    }
                }
            }
            return stores;
        }

        public int Add(Store store)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var query = @"INSERT INTO Stores (StoreName, StoreDate) 
                              VALUES (@name, @date)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", store.StoreName);
                    cmd.Parameters.AddWithValue("@date", store.StoreDate);
                    return cmd.ExecuteNonQuery();
                }
            }
        }


    }
}

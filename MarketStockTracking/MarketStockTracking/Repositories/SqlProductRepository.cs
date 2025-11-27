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

                var query = "SELECT UrunID, UrunAdi, UrunCesidi, EklenmeTarihi FROM Urunler ORDER BY EklenmeTarihi DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        products.Add(new Product
                        {
                            UrunID = rdr.GetInt32(0),
                            UrunAdi = rdr.GetString(1),
                            UrunCesidi = rdr.GetString(2),
                            EklenmeTarihi = rdr.GetDateTime(3)
                        });
                    }
                }
            }

            return products;
        }

        public int Add(Product product)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var query = @"INSERT INTO Urunler (UrunAdi, UrunCesidi, EklenmeTarihi) 
                              VALUES (@ad, @cesit, @tarih)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ad", product.UrunAdi);
                    cmd.Parameters.AddWithValue("@cesit", product.UrunCesidi);
                    cmd.Parameters.AddWithValue("@tarih", product.EklenmeTarihi);

                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

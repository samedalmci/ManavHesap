using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MarketStockTracking
{
    public static class DatabaseHelper
    {
        public static string DbPath = Path.Combine(
            System.AppDomain.CurrentDomain.BaseDirectory,
            "veritabani.db"
        );

        public static string ConnectionString = $"Data Source={DbPath}";

        public static void Initialize()
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();

                // Products tablosu
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Products (
                        ProductID INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductName TEXT NOT NULL,
                        ProductType TEXT,
                        AddedDate TEXT
                    )";
                cmd.ExecuteNonQuery();

                // Stores tablosu
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Stores (
                        StoreID  INTEGER PRIMARY KEY AUTOINCREMENT,
                        StoreName TEXT NOT NULL,
                        StoreDate TEXT
                    )";
                cmd.ExecuteNonQuery();

                // Sales tablosu
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Sales (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductName TEXT,
                        StoreName TEXT,
                        Quantity REAL,
                        NetPrice REAL,
                        GrossPrice REAL,
                        Profit REAL,
                        CashPaid REAL,
                        Debt REAL,
                        CreatedDate TEXT
                    )";
                cmd.ExecuteNonQuery();
            }
        }
    }
}

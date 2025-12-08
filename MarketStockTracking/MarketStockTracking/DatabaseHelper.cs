using Microsoft.Data.Sqlite;
using System.IO;

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

                var cmd = conn.CreateCommand();

                // Products tablosu
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
                        StoreID INTEGER PRIMARY KEY AUTOINCREMENT,
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

                // Settings tablosu
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Settings (
                        Key TEXT PRIMARY KEY,
                        Value TEXT
                    )";
                cmd.ExecuteNonQuery();
            }
        }

        public static string GetSetting(string key, string defaultValue = "")
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Value FROM Settings WHERE Key = @key";
                cmd.Parameters.AddWithValue("@key", key);

                var result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : defaultValue;
            }
        }

        public static void SetSetting(string key, string value)
        {
            using (var conn = new SqliteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO Settings (Key, Value) VALUES (@key, @value)
                    ON CONFLICT(Key) DO UPDATE SET Value = @value";
                cmd.Parameters.AddWithValue("@key", key);
                cmd.Parameters.AddWithValue("@value", value);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
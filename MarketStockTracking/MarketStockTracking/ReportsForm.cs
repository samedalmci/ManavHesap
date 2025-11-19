using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace MarketStockTracking
{
    public partial class ReportsForm : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;";

        public ReportsForm()
        {
            InitializeComponent();   // Designer kontrolleri yükleniyor

            // Event bağlama
            btnAdet.Click += (s, e) => BuildReport("Adet");
            btnBorc.Click += (s, e) => BuildReport("Borc");
            btnMaliyet.Click += (s, e) => BuildReport("Maliyet");

            // İlk açılışta Adet raporu göster
            BuildReport("Adet");
        }


        private void BuildReport(string mode)
        {
            try
            {
                string sql = mode switch
                {
                    "Adet" => @"SELECT Magaza, UrunAdi, SUM(Adet) AS Value FROM Satislar GROUP BY Magaza, UrunAdi ORDER BY Magaza, UrunAdi",
                    "Borc" => @"SELECT Magaza, UrunAdi, SUM(Borc) AS Value FROM Satislar GROUP BY Magaza, UrunAdi ORDER BY Magaza, UrunAdi",
                    "Maliyet" => @"SELECT Magaza, UrunAdi, SUM((Net - Brut) * Adet) AS Value FROM Satislar GROUP BY Magaza, UrunAdi ORDER BY Magaza, UrunAdi",
                    _ => throw new ArgumentException("Bilinmeyen mod")
                };

                var table = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                using (var da = new SqlDataAdapter(cmd))
                {
                    conn.Open();
                    da.Fill(table);
                }

                var products = table.AsEnumerable()
                                    .Select(r => r.Field<string>("UrunAdi"))
                                    .Distinct().OrderBy(x => x).ToList();

                var markets = table.AsEnumerable()
                                   .Select(r => r.Field<string>("Magaza"))
                                   .Distinct().OrderBy(x => x).ToList();

                if (!products.Any() || !markets.Any())
                {
                    dgvReport.DataSource = null;
                    dgvReport.Columns.Clear();
                    dgvReport.Rows.Clear();
                    MessageBox.Show("Gösterilecek veri yok.");
                    return;
                }

                DataTable pivot = new DataTable();
                pivot.Columns.Add("Market", typeof(string));
                foreach (var p in products) pivot.Columns.Add(p, typeof(decimal));
                pivot.Columns.Add("Toplam", typeof(decimal));

                var map = new Dictionary<string, Dictionary<string, decimal>>(StringComparer.OrdinalIgnoreCase);

                foreach (DataRow row in table.Rows)
                {
                    string magaza = row.Field<string>("Magaza");
                    string urun = row.Field<string>("UrunAdi");
                    decimal val = row.Field<object>("Value") == DBNull.Value ? 0m : Convert.ToDecimal(row["Value"]);

                    if (!map.ContainsKey(magaza)) map[magaza] = new Dictionary<string, decimal>();
                    if (!map[magaza].ContainsKey(urun)) map[magaza][urun] = 0m;

                    map[magaza][urun] += val;
                }

                var columnTotals = products.ToDictionary(p => p, p => 0m);
                decimal grandTotal = 0m;

                foreach (var mag in markets)
                {
                    var newRow = pivot.NewRow();
                    newRow["Market"] = mag;

                    decimal rowTotal = 0m;

                    foreach (var prod in products)
                    {
                        decimal v = map.ContainsKey(mag) && map[mag].ContainsKey(prod) ? map[mag][prod] : 0m;

                        newRow[prod] = v;
                        rowTotal += v;
                        columnTotals[prod] += v;
                    }

                    newRow["Toplam"] = rowTotal;
                    grandTotal += rowTotal;

                    pivot.Rows.Add(newRow);
                }

                var totalRow = pivot.NewRow();
                totalRow["Market"] = "Toplam";

                foreach (var prod in products)
                    totalRow[prod] = columnTotals[prod];

                totalRow["Toplam"] = grandTotal;
                pivot.Rows.Add(totalRow);

                dgvReport.DataSource = pivot;

                foreach (DataGridViewColumn col in dgvReport.Columns)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = "N2";
                }

                dgvReport.Columns["Market"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using ClosedXML.Excel;
using System.IO;
// Diğer using'ler (System.Drawing, vb.) gerekirse eklenir.

namespace MarketStockTracking
{
    public partial class ReportsForm : Form
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;";

        public ReportsForm()
        {
            InitializeComponent();   // Designer kontrolleri yükleniyor

            // Başlangıç ve bitiş tarihlerini ayarla
            // Varsayılan olarak başlangıç: 01.01.2024, bitiş: Bugün
            dtpStart.Value = new DateTime(2024, 1, 1);
            dtpEnd.Value = DateTime.Today;

            // Event bağlama
            btnAdet.Click += (s, e) => BuildReport("Adet");
            btnBorc.Click += (s, e) => BuildReport("Borc");
            btnMaliyet.Click += (s, e) => BuildReport("Maliyet");

            // Tarih değiştiğinde de raporu güncelle
            dtpStart.ValueChanged += (s, e) => BuildReport(GetCurrentMode());
            dtpEnd.ValueChanged += (s, e) => BuildReport(GetCurrentMode());

            // İlk açılışta Adet raporu göster
            BuildReport("Adet");
        }

        // Hangi rapor modunun aktif olduğunu takip etmek için (isteğe bağlı, kolaylık için)
        private string currentMode = "Adet";

        private string GetCurrentMode()
        {
            // Bu kısım, hangi düğmenin en son tıklandığını veya hangi raporun gösterildiğini
            // anlamanın bir yolu olmalıdır. Basitçe bir değişken tutabiliriz.
            return currentMode;
        }


        private void BuildReport(string mode)
        {
            try
            {
                currentMode = mode; // Aktif modu kaydet

                // 1. Tarihleri al ve SQL'de güvenli kullanım için ayarla
                // Bitiş tarihi olarak, seçilen günün son anını (23:59:59.997) kullanmak önemlidir 
                // ki o gün yapılan satışlar da dahil edilsin.
                DateTime startDate = dtpStart.Value.Date;
                DateTime endDate = dtpEnd.Value.Date.AddDays(1).AddMilliseconds(-3); // Seçilen günün son anı

                // WHERE koşulunu tanımla. Varsayalım Satislar tablosunda 'Tarih' adında bir sütun var.
                string whereClause = " WHERE EklenmeTarihi >= @StartDate AND EklenmeTarihi < @EndDate ";


                // 2. SQL Sorgularını güncelle
                string sql = mode switch
                {
                    "Adet" => $@"SELECT Magaza, UrunAdi, SUM(Adet) AS Value 
                                FROM Satislar {whereClause} 
                                GROUP BY Magaza, UrunAdi 
                                ORDER BY Magaza, UrunAdi",
                    "Borc" => $@"SELECT Magaza, UrunAdi, SUM(Borc) AS Value 
                                FROM Satislar {whereClause} 
                                GROUP BY Magaza, UrunAdi 
                                ORDER BY Magaza, UrunAdi",
                    "Maliyet" => $@"SELECT Magaza, UrunAdi, SUM((Net - Brut) * Adet) AS Value 
                                FROM Satislar {whereClause} 
                                GROUP BY Magaza, UrunAdi 
                                ORDER BY Magaza, UrunAdi",
                    _ => throw new ArgumentException("Bilinmeyen mod")
                };

                var table = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(sql, conn))
                {
                    // 3. SQL Parametrelerini Ekle
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        da.Fill(table);
                    }
                }

                // --- Kalan Rapor Oluşturma ve Pivotlama Mantığı Aynı Kalacak ---
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

        private void ExportToExcel()
        {
            if (dgvReport.DataSource == null || dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("Dışa aktarılacak veri bulunamadı.");
                return;
            }

            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "Excel Dosyası (*.xlsx)|*.xlsx",
                    FileName = $"Rapor_{DateTime.Now:yyyy_MM_dd}.xlsx"  
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var workbook = new ClosedXML.Excel.XLWorkbook())
                        {
                            var ws = workbook.Worksheets.Add("Rapor");

                            int row = 1;

                            // 🔹 Üst bilgi: tarih aralığı
                            ws.Cell(row, 1).Value = $"Rapor Tarih Aralığı: {dtpStart.Value:dd.MM.yyyy} - {dtpEnd.Value:dd.MM.yyyy}";
                            ws.Range(row, 1, row, dgvReport.Columns.Count).Merge().Style
                                .Font.SetBold()
                                .Font.SetFontSize(12)
                                .Alignment.SetHorizontal(ClosedXML.Excel.XLAlignmentHorizontalValues.Left);

                            row += 2;

                            // 🔹 Başlık satırı
                            for (int i = 0; i < dgvReport.Columns.Count; i++)
                            {
                                var header = ws.Cell(row, i + 1);
                                header.Value = dgvReport.Columns[i].HeaderText;

                                header.Style.Fill.SetBackgroundColor(ClosedXML.Excel.XLColor.FromArgb(0, 90, 158));
                                header.Style.Font.SetFontColor(ClosedXML.Excel.XLColor.White);
                                header.Style.Font.SetBold();
                                header.Style.Alignment.SetHorizontal(ClosedXML.Excel.XLAlignmentHorizontalValues.Center);
                                header.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                            }

                            int startDataRow = row + 1;

                            // 🔹 Verileri yaz
                            for (int i = 0; i < dgvReport.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvReport.Columns.Count; j++)
                                {
                                    var cell = ws.Cell(startDataRow + i, j + 1);
                                    var val = dgvReport.Rows[i].Cells[j].Value;

                                    if (val != null && decimal.TryParse(val.ToString(), out decimal num))
                                        cell.Value = num;
                                    else
                                        cell.Value = val?.ToString() ?? "";

                                    cell.Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;

                                    // Market sütunu sola, sayılar sağa
                                    if (j == 0)
                                        cell.Style.Alignment.SetHorizontal(ClosedXML.Excel.XLAlignmentHorizontalValues.Left);
                                    else
                                        cell.Style.Alignment.SetHorizontal(ClosedXML.Excel.XLAlignmentHorizontalValues.Right);
                                }

                                // 🔹 Toplam satırı ve toplam sütunu renklendirme
                                if (dgvReport.Rows[i].Cells[0].Value?.ToString() == "Toplam")
                                {
                                    ws.Range(startDataRow + i, 1, startDataRow + i, dgvReport.Columns.Count)
                                      .Style.Fill.SetBackgroundColor(ClosedXML.Excel.XLColor.FromArgb(255, 255, 150))
                                      .Font.SetBold();
                                }

                                // 🔹 Toplam sütunu (tüm satırlar)
                                int toplamColIndex = dgvReport.Columns["Toplam"].Index + 1;
                                ws.Cell(startDataRow + i, toplamColIndex)
                                  .Style.Fill.SetBackgroundColor(ClosedXML.Excel.XLColor.FromArgb(255, 255, 150))
                                  .Font.SetBold();
                            }

                            // 🔹 Sütun genişliklerini otomatik ayarla
                            ws.Columns().AdjustToContents();

                            workbook.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Excel dosyası başarıyla oluşturuldu!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }




        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
    }
}
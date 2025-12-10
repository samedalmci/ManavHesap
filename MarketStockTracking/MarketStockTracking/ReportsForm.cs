using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace MarketStockTracking
{
    public partial class ReportsForm : Form
    {
        private string connectionString = DatabaseHelper.ConnectionString;

        public ReportsForm()
        {
            InitializeComponent();

            dtpStart.Value = new DateTime(2024, 1, 1);
            dtpEnd.Value = DateTime.Today;

            btnAdet.Click += (s, e) => BuildReport("Adet");
            btnBorc.Click += (s, e) => BuildReport("Borc");
            btnMaliyet.Click += (s, e) => BuildReport("Maliyet");

            dtpStart.ValueChanged += (s, e) => BuildReport(GetCurrentMode());
            dtpEnd.ValueChanged += (s, e) => BuildReport(GetCurrentMode());

            BuildReport("Adet");
        }

        private string currentMode = "Adet";

        private string GetCurrentMode() => currentMode;

        private void BuildReport(string mode)
        {
            try
            {
                currentMode = mode;

                string startDate = dtpStart.Value.Date.ToString("yyyy-MM-dd HH:mm:ss");
                string endDate = dtpEnd.Value.Date.AddDays(1).AddMilliseconds(-3).ToString("yyyy-MM-dd HH:mm:ss");

                string whereClause = " WHERE CreatedDate >= @StartDate AND CreatedDate < @EndDate ";

                string sql = mode switch
                {
                    "Adet" => $@"SELECT StoreName AS Magaza, ProductName AS UrunAdi, SUM(Quantity) AS Value 
                                 FROM Sales {whereClause} 
                                 GROUP BY StoreName, ProductName 
                                 ORDER BY StoreName, ProductName",
                    "Borc" => $@"SELECT StoreName AS Magaza, ProductName AS UrunAdi, SUM(Debt) AS Value 
                                 FROM Sales {whereClause} 
                                 GROUP BY StoreName, ProductName 
                                 ORDER BY StoreName, ProductName",
                    "Maliyet" => $@"SELECT StoreName AS Magaza, ProductName AS UrunAdi, SUM((NetPrice - GrossPrice) * Quantity) AS Value 
                                     FROM Sales {whereClause} 
                                     GROUP BY StoreName, ProductName 
                                     ORDER BY StoreName, ProductName",
                    _ => throw new ArgumentException("Bilinmeyen mod")
                };

                var table = new DataTable();
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqliteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        using (var reader = cmd.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                    }
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
                        using (var workbook = new XLWorkbook())
                        {
                            var ws = workbook.Worksheets.Add("Rapor");

                            int row = 1;

                            ws.Cell(row, 1).Value = $"Rapor Tarih Aralığı: {dtpStart.Value:dd.MM.yyyy} - {dtpEnd.Value:dd.MM.yyyy}";
                            ws.Range(row, 1, row, dgvReport.Columns.Count).Merge().Style
                                .Font.SetBold()
                                .Font.SetFontSize(12)
                                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                            row += 2;

                            for (int i = 0; i < dgvReport.Columns.Count; i++)
                            {
                                var header = ws.Cell(row, i + 1);
                                header.Value = dgvReport.Columns[i].HeaderText;

                                header.Style.Fill.SetBackgroundColor(XLColor.FromArgb(0, 90, 158));
                                header.Style.Font.SetFontColor(XLColor.White);
                                header.Style.Font.SetBold();
                                header.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                                header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            }

                            int startDataRow = row + 1;

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

                                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                    if (j == 0)
                                        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                    else
                                        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                                }

                                if (dgvReport.Rows[i].Cells[0].Value?.ToString() == "Toplam")
                                {
                                    ws.Range(startDataRow + i, 1, startDataRow + i, dgvReport.Columns.Count)
                                      .Style.Fill.SetBackgroundColor(XLColor.FromArgb(255, 255, 150))
                                      .Font.SetBold();
                                }

                                int toplamColIndex = dgvReport.Columns["Toplam"].Index + 1;
                                ws.Cell(startDataRow + i, toplamColIndex)
                                  .Style.Fill.SetBackgroundColor(XLColor.FromArgb(255, 255, 150))
                                  .Font.SetBold();
                            }

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
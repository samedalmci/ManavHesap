using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using ClosedXML.Excel;

namespace MarketStockTracking
{
    public partial class DebtCancellation : Form
    {
        private string baglanti = DatabaseHelper.ConnectionString;
        private SqliteConnection conn;

        public DebtCancellation()
        {
            InitializeComponent();

            txtMagza.SelectedIndexChanged += txtMagza_SelectedIndexChanged;
            dgvBorclar.CellClick += dgvBorclar_CellClick;

            txtOdenenMiktar.KeyPress += TxtCurrency_KeyPress;
            txtOdenenMiktar.Leave += TxtCurrency_Leave;

            txtKalanBorc.ReadOnly = true;

            try
            {
                conn = new SqliteConnection(baglanti);
                MagazalariYukle();
                LoadDebts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yükleme hatası: " + ex.Message);
            }
        }

        private void MagazalariYukle()
        {
            try
            {
                conn.Open();
                var cmd = new SqliteCommand("SELECT DISTINCT StoreName FROM Stores", conn);
                var dr = cmd.ExecuteReader();

                txtMagza.Items.Clear();
                txtMagza.Items.Add("Tüm Mağazalar");

                while (dr.Read())
                {
                    txtMagza.Items.Add(dr["StoreName"].ToString());
                }

                dr.Close();
                txtMagza.SelectedIndex = 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void txtMagza_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDebts();
            txtKalanBorc.Text = "";
            txtOdenenMiktar.Text = "";
        }

        private void LoadDebts()
        {
            string secilenMagaza = txtMagza.SelectedIndex > 0 ? txtMagza.SelectedItem.ToString() : null;

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string query = "SELECT ID, StoreName AS Magaza, ProductName AS UrunAdi, CreatedDate AS Tarih, CashPaid AS [Ödenen Tutar], Debt AS [Kalan Borç Tutarı] " +
                               "FROM Sales WHERE Debt > 0";

                if (!string.IsNullOrEmpty(secilenMagaza))
                {
                    query += " AND StoreName = @MagazaAdi";
                }

                query += " ORDER BY CreatedDate DESC";

                var cmd = new SqliteCommand(query, conn);
                if (!string.IsNullOrEmpty(secilenMagaza))
                    cmd.Parameters.AddWithValue("@MagazaAdi", secilenMagaza);

                DataTable dt = new DataTable();
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }

                dgvBorclar.DataSource = dt;

                if (dgvBorclar.Columns.Count > 0)
                {
                    var trCulture = new CultureInfo("tr-TR");

                    dgvBorclar.Columns["ID"].HeaderText = "Satış ID";
                    dgvBorclar.Columns["ID"].Width = 60;
                    dgvBorclar.Columns["Tarih"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                    dgvBorclar.Columns["Ödenen Tutar"].DefaultCellStyle.Format = "N2";
                    dgvBorclar.Columns["Kalan Borç Tutarı"].DefaultCellStyle.Format = "N2";
                    dgvBorclar.Columns["Ödenen Tutar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvBorclar.Columns["Kalan Borç Tutarı"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvBorclar.Columns["Ödenen Tutar"].DefaultCellStyle.FormatProvider = trCulture;
                    dgvBorclar.Columns["Kalan Borç Tutarı"].DefaultCellStyle.FormatProvider = trCulture;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Borçlar yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void dgvBorclar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow selectedRow = dgvBorclar.Rows[e.RowIndex];

                string magazaAdi = selectedRow.Cells["Magaza"].Value.ToString();
                string kalanBorcString = selectedRow.Cells["Kalan Borç Tutarı"].Value.ToString();

                txtMagza.Text = magazaAdi;
                txtKalanBorc.Text = kalanBorcString + " TL";
                txtOdenenMiktar.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Satır seçimi hatası: " + ex.Message);
            }
        }

        private void TxtCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)) return;
            if (e.KeyChar == ',' && !txt.Text.Contains(",")) return;
            e.Handled = true;
        }

        private void TxtCurrency_Leave(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string input = txt.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                txt.Text = "";
                return;
            }

            CultureInfo trCulture = new CultureInfo("tr-TR");
            string temizGiris = input.Replace(" TL", "").Replace(".", "");

            if (!temizGiris.Contains(",")) temizGiris += ",00";

            if (decimal.TryParse(temizGiris, NumberStyles.Number, trCulture, out decimal result))
                txt.Text = result.ToString("N2", trCulture) + " TL";
        }

        private decimal ParseCurrencyInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0;

            CultureInfo trCulture = new CultureInfo("tr-TR");
            string temizGiris = input.Trim().Replace(" TL", "");

            if (decimal.TryParse(temizGiris, NumberStyles.Number, trCulture, out decimal result))
                return result;

            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtMagza.SelectedIndex == 0)
            {
                MessageBox.Show("Lütfen mağaza seçin.");
                return;
            }

            if (dgvBorclar.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen ödeme yapmak istediğiniz satırı seçin.");
                return;
            }

            int selectedRowIndex = dgvBorclar.SelectedRows[0].Index;
            int satisID = Convert.ToInt32(dgvBorclar.Rows[selectedRowIndex].Cells["ID"].Value);

            decimal kalanBorc = ParseCurrencyInput(txtKalanBorc.Text);
            decimal odenenMiktar = ParseCurrencyInput(txtOdenenMiktar.Text);

            if (odenenMiktar <= 0)
            {
                MessageBox.Show("Ödenecek miktar sıfırdan büyük olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (odenenMiktar > kalanBorc)
            {
                DialogResult dr = MessageBox.Show(
                    $"Girilen {odenenMiktar.ToString("N2", new CultureInfo("tr-TR"))} TL miktarı, kalan borç olan {kalanBorc.ToString("N2", new CultureInfo("tr-TR"))} TL'den fazladır.\nBorç sıfırlanacak. Yine de devam etmek istiyor musunuz?",
                    "Miktar Uyarısı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                );
                if (dr == DialogResult.No) return;
            }

            decimal yeniBorc = Math.Max(0, kalanBorc - odenenMiktar);

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string updateQuery = "UPDATE Sales SET CashPaid = CashPaid + @OdenenMiktar, Debt = @YeniBorc WHERE ID = @SatisID";
                var cmd = new SqliteCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@OdenenMiktar", odenenMiktar);
                cmd.Parameters.AddWithValue("@YeniBorc", yeniBorc);
                cmd.Parameters.AddWithValue("@SatisID", satisID);

                int affectedRows = cmd.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    MessageBox.Show($"Ödeme başarıyla alındı. Yeni Kalan Borç: {yeniBorc.ToString("N2", new CultureInfo("tr-TR"))} TL", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDebts();
                    txtKalanBorc.Text = "";
                    txtOdenenMiktar.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Borç güncellemesi sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void ExportToExcel()
        {
            if (dgvBorclar.Rows.Count == 0)
            {
                MessageBox.Show("Dışa aktarılacak veri bulunamadı.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Dosyaları (*.xlsx)|*.xlsx|Tüm Dosyalar (*.*)|*.*",
                FileName = $"Borc_Hesaplama_{DateTime.Now:yyyy_MM_dd}.xlsx"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    DataTable dt = new DataTable();
                    foreach (DataGridViewColumn col in dgvBorclar.Columns)
                        dt.Columns.Add(col.HeaderText);

                    foreach (DataGridViewRow row in dgvBorclar.Rows)
                    {
                        if (row.IsNewRow) continue;
                        dt.Rows.Add(row.Cells.Cast<DataGridViewCell>().Select(c => c.Value).ToArray());
                    }

                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Borçlar");

                        for (int i = 0; i < dgvBorclar.Columns.Count; i++)
                        {
                            var header = ws.Cell(1, i + 1);
                            header.Value = dgvBorclar.Columns[i].HeaderText;
                            header.Style.Font.Bold = true;
                            header.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            header.Style.Fill.SetBackgroundColor(XLColor.FromArgb(220, 220, 220));
                            header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }

                        for (int i = 0; i < dgvBorclar.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvBorclar.Columns.Count; j++)
                            {
                                var cell = ws.Cell(i + 2, j + 1);
                                var val = dgvBorclar.Rows[i].Cells[j].Value;

                                if (val != null && decimal.TryParse(val.ToString(), out decimal num))
                                    cell.Value = num;
                                else
                                    cell.Value = val?.ToString() ?? "";

                                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                if (j == 1)
                                    cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                else
                                    cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            }
                        }

                        ws.Columns().AdjustToContents();
                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Borç listesi Excel dosyası olarak kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Excel'e aktarılırken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKalanBorc.Text) || txtKalanBorc.Text == "0,00 TL")
            {
                MessageBox.Show("Borç yok.");
                return;
            }

            txtOdenenMiktar.Text = txtKalanBorc.Text;
        }
    }
}
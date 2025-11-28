using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using MarketStockTracking.Models;
using MarketStockTracking.Repositories;

namespace MarketStockTracking
{
    public partial class SellForm : Form
    {
        string baglanti = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;";
        SqlConnection conn;

        public SellForm()
        {
            InitializeComponent();

            txtUrunAdi.DropDownStyle = ComboBoxStyle.DropDownList;
            txtUrunAdi.Items.Clear();
            txtUrunAdi.Items.Add("Seçiniz...");
            txtUrunAdi.SelectedIndex = 0;

            txtMagza.DropDownStyle = ComboBoxStyle.DropDownList;
            txtMagza.Items.Clear();
            txtMagza.Items.Add("Seçiniz...");
            txtMagza.SelectedIndex = 0;
        }

        private void SellForm_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(baglanti);

            txtBorc.ReadOnly = true;

            txtAdet.KeyPress += TxtAdet_KeyPress;
            txtNet.KeyPress += TxtCurrency_KeyPress;
            txtBrut.KeyPress += TxtCurrency_KeyPress;
            txtPesin.KeyPress += TxtCurrency_KeyPress;

            txtNet.Leave += TxtCurrency_Leave;
            txtBrut.Leave += TxtCurrency_Leave;
            txtPesin.Leave += TxtCurrency_Leave;

            txtNet.Enter += TxtCurrency_Enter;
            txtBrut.Enter += TxtCurrency_Enter;
            txtPesin.Enter += TxtCurrency_Enter;

            UrunleriYukle();
            MagazalariYukle();
            SatisUrunler();
        }

        private void UrunleriYukle()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ProductName FROM Products", conn);
                SqlDataReader dr = cmd.ExecuteReader();

                txtUrunAdi.Items.Clear();
                txtUrunAdi.Items.Add("Seçiniz...");

                while (dr.Read())
                    txtUrunAdi.Items.Add(dr["ProductName"].ToString());

                dr.Close();
                txtUrunAdi.SelectedIndex = 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void MagazalariYukle()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT StoreName FROM Stores", conn);
                SqlDataReader dr = cmd.ExecuteReader();

                txtMagza.Items.Clear();
                txtMagza.Items.Add("Seçiniz...");

                while (dr.Read())
                    txtMagza.Items.Add(dr["StoreName"].ToString());

                dr.Close();
                txtMagza.SelectedIndex = 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void TxtAdet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void TxtCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar)) return;
            if (e.KeyChar == ',' && !txt.Text.Contains(",")) return;
            e.Handled = true;
        }

        private void TxtCurrency_Enter(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (!string.IsNullOrWhiteSpace(txt.Text))
            {
                txt.Text = txt.Text.Replace(" TL", "").Trim();
                txt.SelectAll();
            }
        }

        private void TxtCurrency_Leave(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            string input = txt.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) { txt.Text = ""; return; }

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
            try
            {
                if (txtUrunAdi.SelectedIndex == 0) { MessageBox.Show("Lütfen ürün seçin."); return; }
                if (txtMagza.SelectedIndex == 0) { MessageBox.Show("Lütfen mağaza seçin."); return; }
                if (string.IsNullOrWhiteSpace(txtAdet.Text)) { MessageBox.Show("Lütfen adet girin."); return; }

                decimal adet = decimal.TryParse(txtAdet.Text, out decimal a) ? a : 0;
                decimal net = ParseCurrencyInput(txtNet.Text);
                decimal brut = ParseCurrencyInput(txtBrut.Text);
                decimal pesin = ParseCurrencyInput(txtPesin.Text);

                if (net == 0) { MessageBox.Show("Lütfen geçerli Net fiyat girin."); return; }
                if (brut == 0) { MessageBox.Show("Lütfen geçerli Alış fiyatı girin."); return; }

                decimal kar = adet * (net - brut);
                decimal borc = Math.Max(0, (adet * net) - pesin);
                decimal toplamTutar = adet * net;

                if (pesin > toplamTutar)
                {
                    DialogResult dr = MessageBox.Show(
                        "Girdiğiniz peşinat toplam satış tutarından fazla.\n" +
                        "Peşinat tutarı toplam tutara eşitlenecek ve borç 0 olacaktır. Yine de kaydetmek istiyor musunuz?",
                        "Peşinat Uyarısı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                    );
                    if (dr == DialogResult.No) return;
                    pesin = toplamTutar;
                    borc = 0;
                }

                Sale sale = new Sale
                {
                    ProductName = txtUrunAdi.Text,
                    StoreName = txtMagza.Text,
                    Quantity = adet,
                    NetPrice = net,
                    GrossPrice = brut,
                    Profit = kar,
                    CashPaid = pesin,
                    Debt = borc,
                    CreatedDate = DateTime.Now
                };

                SqlSalesRepository repo = new SqlSalesRepository(baglanti);
                repo.Add(sale);

                MessageBox.Show("Satış eklendi.");

                // Fiş yazdır
                ReceiptPrinter printer = new ReceiptPrinter(
                    sale.ProductName, sale.Quantity, sale.NetPrice, sale.CashPaid, sale.Debt, sale.StoreName
                );
                printer.Bas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
            finally
            {
                SatisUrunler();
            }
        }

        private void ProfitCalculation()
        {
            CultureInfo trCulture = new CultureInfo("tr-TR");
            decimal adet = decimal.TryParse(txtAdet.Text, out decimal a) ? a : 0;
            decimal net = ParseCurrencyInput(txtNet.Text);
            decimal brut = ParseCurrencyInput(txtBrut.Text);
            decimal sonuc = adet * (net - brut);
            txtKarZarar.Text = sonuc.ToString("N2", trCulture) + " TL";
            txtKarZarar.ForeColor = sonuc < 0 ? Color.Red : Color.Green;
        }

        private void DebtCalculation()
        {
            CultureInfo trCulture = new CultureInfo("tr-TR");
            decimal adet = decimal.TryParse(txtAdet.Text, out decimal a) ? a : 0;
            decimal net = ParseCurrencyInput(txtNet.Text);
            decimal pesin = ParseCurrencyInput(txtPesin.Text);
            decimal kalan = (adet * net) - pesin;
            if (kalan < 0) kalan = 0;
            txtBorc.Text = kalan.ToString("N2", trCulture) + " TL";
            txtBorc.ForeColor = kalan > 0 ? Color.Red : Color.Black;
        }

        private void SatisUrunler()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Sales", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUrunler.DataSource = dt;

                // DataGridView formatlama ve başlıkları Türkçe yap
                foreach (DataGridViewColumn col in dgvUrunler.Columns)
                {
                    switch (col.Name)
                    {
                        case "ProductName":
                            col.HeaderText = "Ürün Adı";
                            break;
                        case "StoreName":
                            col.HeaderText = "Mağaza";
                            break;
                        case "Quantity":
                            col.HeaderText = "Adet";
                            col.DefaultCellStyle.Format = "N2";
                            break;
                        case "NetPrice":
                            col.HeaderText = "Net Fiyat";
                            col.DefaultCellStyle.Format = "N2";
                            break;
                        case "CostPrice":
                            col.HeaderText = "Alış Fiyatı";
                            col.DefaultCellStyle.Format = "N2";
                            break;
                        case "Profit":
                            col.HeaderText = "Kar/Zarar";
                            col.DefaultCellStyle.Format = "N2";
                            break;
                        case "CashPaid":
                            col.HeaderText = "Peşinat";
                            col.DefaultCellStyle.Format = "N2";
                            break;
                        case "Debt":
                            col.HeaderText = "Borç";
                            col.DefaultCellStyle.Format = "N2";
                            break;
                        case "CreatedDate":
                            col.HeaderText = "Tarih";
                            col.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
                            break;
                    }
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }


         private void txtNet_TextChanged(object sender, EventArgs e) { ProfitCalculation(); DebtCalculation(); }
         private void txtBrut_TextChanged(object sender, EventArgs e) { ProfitCalculation(); }
         private void txtPesin_TextChanged(object sender, EventArgs e) { DebtCalculation(); }
         private void txtAdet_TextChanged(object sender, EventArgs e) { ProfitCalculation(); DebtCalculation(); }

         private void txtBorc_TextChanged(object sender, EventArgs e) { }

         private void ExportToExcel()
         {
            if (dgvUrunler.Rows.Count == 0) { MessageBox.Show("Dışa aktarılacak veri bulunamadı."); return; }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Dosyaları (*.xlsx)|*.xlsx|Tüm Dosyalar (*.*)|*.*",
                FileName = $"Satis_Listesi_{DateTime.Now:yyyy_MM_dd}.xlsx"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    DataTable dt = new DataTable();
                    foreach (DataGridViewColumn col in dgvUrunler.Columns)
                        dt.Columns.Add(col.HeaderText);

                    foreach (DataGridViewRow row in dgvUrunler.Rows)
                    {
                        if (row.IsNewRow) continue;
                        dt.Rows.Add(row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString() ?? "").ToArray());
                    }

                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Satışlar");

                        for (int i = 0; i < dgvUrunler.Columns.Count; i++)
                        {
                            var header = ws.Cell(1, i + 1);
                            header.Value = dgvUrunler.Columns[i].HeaderText;
                            header.Style.Font.Bold = true;
                            header.Style.Fill.SetBackgroundColor(XLColor.FromArgb(0, 90, 158));
                            header.Style.Font.SetFontColor(XLColor.White);
                            header.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }

                        for (int i = 0; i < dgvUrunler.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvUrunler.Columns.Count; j++)
                            {
                                var cell = ws.Cell(i + 2, j + 1);
                                cell.Value = dgvUrunler.Rows[i].Cells[j].Value?.ToString() ?? "";
                                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                if (j == 0 || j == 1)
                                    cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                else
                                    cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            }

                            if (i % 2 == 0)
                                ws.Range(i + 2, 1, i + 2, dgvUrunler.Columns.Count)
                                    .Style.Fill.SetBackgroundColor(XLColor.FromArgb(240, 240, 240));
                        }

                        ws.Columns().AdjustToContents();
                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Satış listesi Excel dosyası olarak kaydedildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Excel aktarılırken hata: " + ex.Message);
                }
            }

         }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
    }
}

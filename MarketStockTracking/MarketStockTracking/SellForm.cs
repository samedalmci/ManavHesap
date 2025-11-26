using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace MarketStockTracking
{
    public partial class SellForm : Form
    {
        // SQL bağlantı dizesi
        string baglanti = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;";
        SqlConnection conn;

        public SellForm()
        {
            InitializeComponent();

            // HATA ÇÖZÜMÜ: conn oluşturma ve SatisUrunler() çağrısı buradan silindi!

            // ComboBox'ların başlangıç ayarları (Bunlar kalabilir, veritabanı işlemi yapmaz)
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
            // HATA ÇÖZÜMÜ: Veritabanı işlemleri buraya taşındı!
            conn = new SqlConnection(baglanti);

            txtBorc.ReadOnly = true;

            // KeyPress olayları (Zaten var)
            txtAdet.KeyPress += TxtAdet_KeyPress;
            txtNet.KeyPress += TxtCurrency_KeyPress;
            txtBrut.KeyPress += TxtCurrency_KeyPress;
            txtPesin.KeyPress += TxtCurrency_KeyPress;

            // Leave olayları (Zaten var)
            txtNet.Leave += TxtCurrency_Leave;
            txtBrut.Leave += TxtCurrency_Leave;
            txtPesin.Leave += TxtCurrency_Leave;

            // Enter olayları (Zaten var)
            txtNet.Enter += TxtCurrency_Enter;
            txtBrut.Enter += TxtCurrency_Enter;
            txtPesin.Enter += TxtCurrency_Enter;

            // Veritabanından veri çekme metotları buraya taşındı
            UrunleriYukle();
            MagazalariYukle();
            SatisUrunler(); // Satış listesi de Form_Load'da listeleniyor
        }

        private void UrunleriYukle()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UrunAdi FROM Urunler", conn);
                SqlDataReader dr = cmd.ExecuteReader();

                txtUrunAdi.Items.Clear();
                txtUrunAdi.Items.Add("Seçiniz...");

                while (dr.Read())
                {
                    txtUrunAdi.Items.Add(dr["UrunAdi"].ToString());
                }

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
                SqlCommand cmd = new SqlCommand("SELECT MagazaAdi FROM Magazalar", conn);
                SqlDataReader dr = cmd.ExecuteReader();

                txtMagza.Items.Clear();
                txtMagza.Items.Add("Seçiniz...");

                while (dr.Read())
                {
                    txtMagza.Items.Add(dr["MagazaAdi"].ToString());
                }

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

            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                return;

            if (e.KeyChar == ',' && !txt.Text.Contains(","))
                return;

            e.Handled = true;
        }

        // **********************************************
        // * GÜNCEL METOT: ALANA GİRİNCE FORMATI SİL *
        // **********************************************
        private void TxtCurrency_Enter(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if (!string.IsNullOrWhiteSpace(txt.Text))
            {
                txt.Text = txt.Text.Replace(" TL", "").Trim();
                txt.SelectAll();
            }
        }

        // **********************************************
        // * GÜNCEL METOT: ALANDAN ÇIKINCA OTOMATİK FORMAT *
        // **********************************************
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

            if (!temizGiris.Contains(","))
            {
                temizGiris += ",00";
            }

            if (decimal.TryParse(temizGiris, NumberStyles.Number, trCulture, out decimal result))
            {
                txt.Text = result.ToString("N2", trCulture) + " TL";
            }
        }

        // **********************************************
        // * GÜNCEL YARDIMCI METOT: PARSE İŞLEMİ *
        // **********************************************
        private decimal ParseCurrencyInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            CultureInfo trCulture = new CultureInfo("tr-TR");
            string temizGiris = input.Trim().Replace(" TL", "");

            if (decimal.TryParse(temizGiris, NumberStyles.Number, trCulture, out decimal result))
            {
                return result;
            }

            return 0;
        }

        // **********************************************
        // * ANA İŞ METOTLARI *
        // **********************************************

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Eksik alan kontrolleri
                if (txtUrunAdi.SelectedIndex == 0) { MessageBox.Show("Lütfen ürün seçin."); return; }
                if (txtMagza.SelectedIndex == 0) { MessageBox.Show("Lütfen mağaza seçin."); return; }
                if (string.IsNullOrWhiteSpace(txtAdet.Text)) { MessageBox.Show("Lütfen adet girin."); return; }

                // Parse işlemleri
                decimal adet = decimal.TryParse(txtAdet.Text, out decimal a) ? a : 0;
                decimal net = ParseCurrencyInput(txtNet.Text);
                decimal brut = ParseCurrencyInput(txtBrut.Text);
                decimal pesin = ParseCurrencyInput(txtPesin.Text);

                // Eğer Parse metotlarından sonra hala sıfır ise kullanıcıya uyarı verilir
                if (net == 0) { MessageBox.Show("Lütfen geçerli Net fiyat girin."); return; }
                if (brut == 0) { MessageBox.Show("Lütfen geçerli Alış fiyatı girin."); return; }
                if (pesin == 0 && adet * net > 0)
                {
                    // Peşinat 0 olabilir, ancak toplam tutar > 0 ise kullanıcıya uyarı gösterilebilir
                    // Bu kontrolü yoruma alıyorum, çünkü peşin 0 olabilir (tamamen borçlu satış)
                }

                CultureInfo trCulture = new CultureInfo("tr-TR");

                // Kar ve Borç alanları ekrandan formatlı olarak alınır
                // Not: Hesaplamayı tekrar yapmak, ekrandan okumaktan daha güvenli olabilir.
                decimal kar = adet * (net - brut);
                decimal borc = Math.Max(0, (adet * net) - pesin); // Borç negatif olamaz

                decimal toplamTutar = adet * net;

                // Peşinat kontrolü
                if (pesin > toplamTutar)
                {
                    DialogResult dr = MessageBox.Show(
                        "Girdiğiniz peşinat toplam satış tutarından fazla.\n" +
                        "Peşinat tutarı toplam tutara eşitlenecek ve borç 0 olacaktır. Yine de kaydetmek istiyor musunuz?",
                        "Peşinat Uyarısı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (dr == DialogResult.No)
                    {
                        MessageBox.Show("Kayıt iptal edildi.");
                        return;
                    }
                    // Peşinatı düzeltip borcu sıfırlıyoruz
                    pesin = toplamTutar;
                    borc = 0;
                }

                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Satislar (UrunAdi, Magaza, Adet, Net, Brut, Kar, Pesin, Borc, EklenmeTarihi) VALUES (@ad,@m,@adet,@n,@b,@k,@p,@borc,@t)", conn);

                cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text);
                cmd.Parameters.AddWithValue("@m", txtMagza.Text);
                cmd.Parameters.AddWithValue("@adet", adet);
                cmd.Parameters.AddWithValue("@n", net);
                cmd.Parameters.AddWithValue("@b", brut);
                cmd.Parameters.AddWithValue("@k", kar);
                cmd.Parameters.AddWithValue("@p", pesin);
                cmd.Parameters.AddWithValue("@borc", borc);
                cmd.Parameters.AddWithValue("@t", DateTime.Now);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Satış eklendi.");


                // Fiş için gerekli verileri topla
                string urunAdi = txtUrunAdi.Text;
                decimal birimFiyat = net; // Net fiyatı birim fiyat olarak alıyoruz
                decimal pesinMiktar = pesin;
                string magzaAdi = txtMagza.Text;

                // Borç hesaplama
                //decimal borc = Math.Max(0, (adet * net) - pesin);

                // ReceiptPrinter nesnesini oluştur
                ReceiptPrinter printer = new ReceiptPrinter(urunAdi, adet, birimFiyat, pesinMiktar, borc, magzaAdi);

                // Eğer Satislar tablosundan ID dönüyorsa, onu atayabilirsiniz
                // Örnek: printer.SatisNo = insertedSatisId;

                // Fişi yazdır
                printer.Bas();


            }
            catch (SqlException sqlex)
            {
                MessageBox.Show($"Veritabanı Hatası: {sqlex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmedik Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
                SatisUrunler(); // Kayıt sonrası listeyi yeniden yükle
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
            if (kalan < 0) kalan = 0; // Borç negatif olamaz (fazla ödeme durumunda sıfırlanır)

            txtBorc.Text = kalan.ToString("N2", trCulture) + " TL";
            txtBorc.ForeColor = kalan > 0 ? Color.Red : Color.Black;
        }

        private void SatisUrunler()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Satislar", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUrunler.DataSource = dt;

                // DataGridView formatlama
                if (dgvUrunler.Columns.Contains("Net"))
                    dgvUrunler.Columns["Net"].DefaultCellStyle.Format = "N2";
                if (dgvUrunler.Columns.Contains("Brut"))
                    dgvUrunler.Columns["Brut"].DefaultCellStyle.Format = "N2";
                if (dgvUrunler.Columns.Contains("Kar"))
                    dgvUrunler.Columns["Kar"].DefaultCellStyle.Format = "N2";
                if (dgvUrunler.Columns.Contains("Pesin"))
                    dgvUrunler.Columns["Pesin"].DefaultCellStyle.Format = "N2";
                if (dgvUrunler.Columns.Contains("Borc"))
                    dgvUrunler.Columns["Borc"].DefaultCellStyle.Format = "N2";
                if (dgvUrunler.Columns.Contains("EklenmeTarihi"))
                    dgvUrunler.Columns["EklenmeTarihi"].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";

            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        // **********************************************
        // * TEXTCHANGED OLAYLARI (Sadece Hesaplama Tetikler) *
        // **********************************************

        private void txtNet_TextChanged(object sender, EventArgs e)
        {
            ProfitCalculation();
            DebtCalculation();
        }

        private void txtBrut_TextChanged(object sender, EventArgs e)
        {
            ProfitCalculation();
        }

        private void txtPesin_TextChanged(object sender, EventArgs e)
        {
            DebtCalculation();
        }

        private void txtAdet_TextChanged(object sender, EventArgs e)
        {
            ProfitCalculation();
            DebtCalculation();
        }

        private void txtBorc_TextChanged(object sender, EventArgs e)
        {
        }

        // **********************************************
        // * EXCEL İŞLEMLERİ *
        // **********************************************

        private void ExportToExcel()
        {
            if (dgvUrunler.Rows.Count == 0)
            {
                MessageBox.Show("Dışa aktarılacak veri bulunamadı.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Dosyaları (*.xlsx)|*.xlsx|Tüm Dosyalar (*.*)|*.*",
                FileName = $"Satis_Listesi_{DateTime.Now:yyyy_MM_dd}.xlsx"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    // DataTable oluştur
                    DataTable dt = new DataTable();
                    foreach (DataGridViewColumn col in dgvUrunler.Columns)
                        dt.Columns.Add(col.HeaderText);

                    foreach (DataGridViewRow row in dgvUrunler.Rows)
                    {
                        if (row.IsNewRow) continue;
                        dt.Rows.Add(row.Cells.Cast<DataGridViewCell>().Select(c => c.Value).ToArray());
                    }

                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Satışlar");

                        // Başlık satırı
                        for (int i = 0; i < dgvUrunler.Columns.Count; i++)
                        {
                            var header = ws.Cell(1, i + 1);
                            header.Value = dgvUrunler.Columns[i].HeaderText;
                            header.Style.Font.Bold = true;
                            header.Style.Fill.SetBackgroundColor(XLColor.FromArgb(0, 90, 158)); // koyu mavi
                            header.Style.Font.SetFontColor(XLColor.White);
                            header.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            header.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }

                        // Verileri yaz
                        for (int i = 0; i < dgvUrunler.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvUrunler.Columns.Count; j++)
                            {
                                var cell = ws.Cell(i + 2, j + 1);
                                var val = dgvUrunler.Rows[i].Cells[j].Value;

                                if (val != null && decimal.TryParse(val.ToString(), out decimal num))
                                    cell.Value = num;
                                else
                                    cell.Value = val?.ToString() ?? "";

                                // Kenarlık
                                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                                // Hizalama: Market ve Ürün sütunları sola, sayılar sağa
                                if (j == 0 || j == 1) // sütun indexini kontrol et (Market ve Ürün)
                                    cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                                else
                                    cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                            }

                            // Satırı renklendirmek istersen örnek:
                            if (i % 2 == 0) // çift satırlar hafif gri
                                ws.Range(i + 2, 1, i + 2, dgvUrunler.Columns.Count)
                                    .Style.Fill.SetBackgroundColor(XLColor.FromArgb(240, 240, 240));
                        }

                        ws.Columns().AdjustToContents();

                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Satış listesi Excel dosyası olarak başarıyla kaydedildi.",
                        "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Excel’e aktarılırken bir hata oluştu: " + ex.Message,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
    }
}
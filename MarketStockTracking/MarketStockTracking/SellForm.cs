using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace MarketStockTracking
{
    public partial class SellForm : Form
    {
        // Bağlantı dizesi
        string baglanti = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;";
        SqlConnection conn;

        public SellForm()
        {
            InitializeComponent();
            conn = new SqlConnection(baglanti);
            SatisUrunler(); // Form açılırken verileri yükle
        }

        private void SellForm_Load(object sender, EventArgs e)
        {
            // Borç alanını salt okunur yap
            txtBorc.ReadOnly = true;

            // Adet alanına sadece tam sayı kuralı
            txtAdet.KeyPress += TxtAdet_KeyPress;

            // Para birimi alanlarına sayı ve bir virgül kuralını bağla
            txtNet.KeyPress += TxtCurrency_KeyPress;
            txtBrut.KeyPress += TxtCurrency_KeyPress;
            txtPesin.KeyPress += TxtCurrency_KeyPress;
        }

        // --- UX METODU: Sadece Sayı Girişi İzni (Adet Alanı) ---
        private void TxtAdet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // --- UX METODU: Fiyat Alanları için Sayı ve Virgül İzni ---
        private void TxtCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // Türkçe ondalık ayıracı olan virgüle (,) izin ver
            if (e.KeyChar == Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) && !((TextBox)sender).Text.Contains(","))
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }

        // --- BUTON TIKLAMA (KAYIT) ---
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Basit bir ön kontrol
                if (string.IsNullOrWhiteSpace(txtUrunAdi.Text) || string.IsNullOrWhiteSpace(txtAdet.Text) || txtAdet.Text == "0")
                {
                    MessageBox.Show("Lütfen Ürün Adı ve geçerli bir Adet girin.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // --- Gerekli Değerleri Temizleyip Alma ---

                // Adet, Net ve Brüt değerlerini al
                decimal adet = int.TryParse(txtAdet.Text, out int a) ? a : 0;
                decimal net = decimal.TryParse(txtNet.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal n) ? n : 0;
                decimal brut = decimal.TryParse(txtBrut.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal b) ? b : 0;

                // Kar/Zarar değerini alıp temizleme
                string karZararText = txtKarZarar.Text.Replace(" TL", "");
                decimal karValue;
                bool karParsed = decimal.TryParse(
                    karZararText,
                    NumberStyles.Currency,
                    new CultureInfo("tr-TR"),
                    out karValue);

                // Peşin ve Borç değerlerini al
                decimal pesin = decimal.TryParse(txtPesin.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal p) ? p : 0;
                decimal borc = decimal.TryParse(txtBorc.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal bo) ? bo : 0;

                // Toplam Satış Tutarı (Kontrol için gerekli)
                decimal toplamSatisTutari = adet * net;

                // --- FAZLA ÖDEME KONTROLÜ (Yeni Eklenen Bölüm) ---
                if (pesin > toplamSatisTutari)
                {
                    string onayMesaji = string.Format(
                        new CultureInfo("tr-TR"),
                        "Girilen Peşinat Tutarı ({0:N2} TL), Toplam Satış Tutarı'nı ({1:N2} TL) aşıyor.\nFazla ödeme yapılıyor. Kaydı onaylıyor musunuz?",
                        pesin,
                        toplamSatisTutari);

                    DialogResult result = MessageBox.Show(
                        onayMesaji,
                        "Fazla Ödeme Onayı",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                    {
                        // Kullanıcı Hayır derse, kaydetme işlemini durdur
                        return;
                    }
                }
                // --- KONTROL SONU ---


                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Satislar (UrunAdi, Magaza, Adet, Net, Brut, Kar, Pesin, Borc) " +
                    "VALUES (@ad, @magza, @adet, @net, @brut, @kar, @pesin, @borc)", conn);

                cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text);
                cmd.Parameters.AddWithValue("@magza", txtMagza.Text);
                cmd.Parameters.AddWithValue("@adet", adet);
                cmd.Parameters.AddWithValue("@net", net);
                cmd.Parameters.AddWithValue("@brut", brut);
                cmd.Parameters.AddWithValue("@kar", karParsed ? karValue : 0);
                cmd.Parameters.AddWithValue("@pesin", pesin);
                cmd.Parameters.AddWithValue("@borc", borc);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Satış başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show($"Veritabanı Hatası: Kayıt sırasında bir sorun oluştu.\n{sqlex.Message}", "Kayıt Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Beklenmedik Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                SatisUrunler(); // Kayıt sonrası tabloyu yenile
            }
        }

        // --- FONKSIYONLAR ---

        private void ProfitCalculation()
        {
            decimal adet = 0;
            decimal net = 0;
            decimal brut = 0;

            decimal.TryParse(txtAdet.Text, out adet);

            string netText = txtNet.Text.Replace(" TL", "");
            string brutText = txtBrut.Text.Replace(" TL", "");

            decimal.TryParse(netText, NumberStyles.Currency, new CultureInfo("tr-TR"), out net);
            decimal.TryParse(brutText, NumberStyles.Currency, new CultureInfo("tr-TR"), out brut);

            // Hesaplama: Adet * (Satış Fiyatı - Maliyet Fiyatı)
            decimal sonuc = adet * (net - brut);

            txtKarZarar.Text = sonuc.ToString("N2", new CultureInfo("tr-TR")) + " TL";

            txtKarZarar.ForeColor = sonuc < 0 ? Color.Red : Color.Green;
        }

        private void DebtCalculation()
        {
            decimal adet = 0;
            decimal net = 0; // Birim Satış Fiyatı
            decimal pesin = 0; // Peşin ödenen miktar

            decimal.TryParse(txtAdet.Text, out adet);

            // Net Fiyatı (Satış Fiyatı) formatından temizle
            string netText = txtNet.Text.Replace(" TL", "");
            decimal.TryParse(netText, NumberStyles.Currency, new CultureInfo("tr-TR"), out net);

            // Peşin Fiyatı formatından temizle
            string pesinText = txtPesin.Text.Replace(" TL", "");
            decimal.TryParse(pesinText, NumberStyles.Currency, new CultureInfo("tr-TR"), out pesin);

            // Müşterinin toplam ödemesi gereken tutar (Net Fiyat * Adet)
            decimal toplamSatisTutari = adet * net;

            // Kalan Borç = Toplam Tutar - Peşinat
            decimal kalanBorc = toplamSatisTutari - pesin;

            // Borç hiçbir zaman negatif olamaz (Fazla ödeme yapıldıysa borç 0'dır)
            if (kalanBorc < 0)
            {
                kalanBorc = 0;
            }

            // Sonucu txtBorc alanına formatlı şekilde yaz
            txtBorc.Text = kalanBorc.ToString("N2", new CultureInfo("tr-TR")) + " TL";

            // Borç varsa kırmızı renkte göster
            txtBorc.ForeColor = kalanBorc > 0 ? Color.Red : Color.Black;
        }

        // --- SATIŞ VERİLERİNİ YÜKLEME VE GÖSTERME METODU (Düzeltildi) ---
        private void SatisUrunler()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Satislar", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Hata aldığınız yerdeki önemli satır: Verinin tabloya atanması
                // Eğer DataGridView kontrolünüzün adı 'dgvUrunler' değilse, lütfen bu adı değiştirin.
                dgvUrunler.DataSource = dt;
            }
            catch (Exception ex)
            {
                // Veri yüklenemezse kullanıcıyı bilgilendir
                MessageBox.Show("Satış verileri yüklenirken bir hata oluştu: " + ex.Message, "Veri Yükleme Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // --- TEXTCHANGED OLAYLARI ---

        private void txtNet_TextChanged(object sender, EventArgs e)
        {
            if (txtNet.Text == "")
            {
                ProfitCalculation();
                DebtCalculation();
                return;
            }

            string temiz = txtNet.Text.Replace(".", "").Replace(",", "").Replace(" TL", "");

            if (decimal.TryParse(temiz, out decimal deger))
            {
                txtNet.TextChanged -= txtNet_TextChanged;
                txtNet.Text = string.Format(new CultureInfo("tr-TR"), "{0:N2} TL", deger / 100);
                txtNet.SelectionStart = txtNet.Text.Length - 3;
                txtNet.TextChanged += txtNet_TextChanged;
            }

            ProfitCalculation();
            DebtCalculation();
        }

        private void txtBrut_TextChanged(object sender, EventArgs e)
        {
            if (txtBrut.Text == "")
            {
                ProfitCalculation();
                return;
            }

            string temiz = txtBrut.Text.Replace(".", "").Replace(",", "").Replace(" TL", "");

            if (decimal.TryParse(temiz, out decimal deger))
            {
                txtBrut.TextChanged -= txtBrut_TextChanged;
                txtBrut.Text = string.Format(new CultureInfo("tr-TR"), "{0:N2} TL", deger / 100);
                txtBrut.SelectionStart = txtBrut.Text.Length - 3;
                txtBrut.TextChanged += txtBrut_TextChanged;
            }

            ProfitCalculation();
        }

        private void txtAdet_TextChanged(object sender, EventArgs e)
        {
            ProfitCalculation();
            DebtCalculation();
        }

        private void txtPesin_TextChanged(object sender, EventArgs e)
        {
            if (txtPesin.Text == "")
            {
                DebtCalculation();
                return;
            }

            string temiz = txtPesin.Text.Replace(".", "").Replace(",", "").Replace(" TL", "");

            if (decimal.TryParse(temiz, out decimal deger))
            {
                txtPesin.TextChanged -= txtPesin_TextChanged;
                txtPesin.Text = string.Format(new CultureInfo("tr-TR"), "{0:N2} TL", deger / 100);
                txtPesin.SelectionStart = txtPesin.Text.Length - 3;
                txtPesin.TextChanged += txtPesin_TextChanged;
            }

            DebtCalculation();
        }

        private void txtBorc_TextChanged(object sender, EventArgs e)
        {
            // Borç alanı ReadOnly olduğu için manuel giriş yoktur.
        }
    }
}
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MarketStockTracking
{
    public partial class SellForm : Form
    {
        string baglanti = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;";
        SqlConnection conn;

        public SellForm()
        {
            InitializeComponent();
            conn = new SqlConnection(baglanti);
            SatisUrunler();

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
            txtBorc.ReadOnly = true;

            txtAdet.KeyPress += TxtAdet_KeyPress;
            txtNet.KeyPress += TxtCurrency_KeyPress;
            txtBrut.KeyPress += TxtCurrency_KeyPress;
            txtPesin.KeyPress += TxtCurrency_KeyPress;

            UrunleriYukle();
            MagazalariYukle();
        }

        private void UrunleriYukle()
        {
            try
            {
                conn.Open();
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
                conn.Close();
            }
        }

        private void MagazalariYukle()
        {
            try
            {
                conn.Open();
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
                conn.Close();
            }
        }

        private void TxtAdet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void TxtCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                return;

            if (e.KeyChar == ',' && !((TextBox)sender).Text.Contains(","))
                return;

            e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUrunAdi.SelectedIndex == 0)
                {
                    MessageBox.Show("Lütfen ürün seçin.");
                    return;
                }

                if (txtMagza.SelectedIndex == 0)
                {
                    MessageBox.Show("Lütfen mağaza seçin.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAdet.Text))
                {
                    MessageBox.Show("Lütfen adet girin.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNet.Text))
                {
                    MessageBox.Show("Net fiyat girin.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtBrut.Text))
                {
                    MessageBox.Show("Alış fiyatı girin.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPesin.Text))
                {
                    MessageBox.Show("Peşinat girin.");
                    return;
                }

                decimal adet = decimal.Parse(txtAdet.Text);
                decimal net = decimal.Parse(txtNet.Text.Replace(" TL", ""), new CultureInfo("tr-TR"));
                decimal brut = decimal.Parse(txtBrut.Text.Replace(" TL", ""), new CultureInfo("tr-TR"));
                decimal kar = decimal.Parse(txtKarZarar.Text.Replace(" TL", ""), new CultureInfo("tr-TR"));
                decimal pesin = decimal.Parse(txtPesin.Text.Replace(" TL", ""), new CultureInfo("tr-TR"));
                decimal borc = decimal.Parse(txtBorc.Text.Replace(" TL", ""), new CultureInfo("tr-TR"));

                conn.Open();
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
                SatisUrunler();
            }
        }

        private void ProfitCalculation()
        {
            decimal adet = decimal.TryParse(txtAdet.Text, out decimal a) ? a : 0;
            decimal net = decimal.TryParse(txtNet.Text.Replace(" TL", ""), out decimal n) ? n : 0;
            decimal brut = decimal.TryParse(txtBrut.Text.Replace(" TL", ""), out decimal b) ? b : 0;

            decimal sonuc = adet * (net - brut);

            txtKarZarar.Text = sonuc.ToString("N2", new CultureInfo("tr-TR")) + " TL";
            txtKarZarar.ForeColor = sonuc < 0 ? Color.Red : Color.Green;
        }

        private void DebtCalculation()
        {
            decimal adet = decimal.TryParse(txtAdet.Text, out decimal a) ? a : 0;
            decimal net = decimal.TryParse(txtNet.Text.Replace(" TL", ""), out decimal n) ? n : 0;
            decimal pesin = decimal.TryParse(txtPesin.Text.Replace(" TL", ""), out decimal p) ? p : 0;

            decimal kalan = (adet * net) - pesin;
            if (kalan < 0) kalan = 0;

            txtBorc.Text = kalan.ToString("N2", new CultureInfo("tr-TR")) + " TL";
            txtBorc.ForeColor = kalan > 0 ? Color.Red : Color.Black;
        }

        private void SatisUrunler()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Satislar", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUrunler.DataSource = dt;
            }
            finally
            {
                conn.Close();
            }
        }

        // -----------------------------
        //       YENİ FORMAT SİSTEMİ
        // -----------------------------

        private void FormatTL(TextBox txt, EventHandler handler)
        {
            if (!txt.Focused) return;

            // 1. TextChanged olayını kaldır ki döngüye girmesin.
            txt.TextChanged -= handler;

            // 2. Metin kutusundaki formatlı değeri al, " TL" ve tüm binlik ayırıcıları (".") kaldır.
            string girisMetni = txt.Text
                .Replace(" TL", "")
                .Replace(".", "") // Binlik ayırıcıları kaldır (tr-TR'de nokta)
                .Trim();

            // 3. Sadece sayıları ve bir adet virgülü (ondalık ayırıcı) kabul et
            string sadeceSayiVeVirgul = "";
            bool virgulKullanildi = false;
            foreach (char c in girisMetni)
            {
                if (char.IsDigit(c))
                {
                    sadeceSayiVeVirgul += c;
                }
                // Sadece bir tane ondalık ayırıcıya (virgül) izin ver
                else if (c == ',' && !virgulKullanildi)
                {
                    sadeceSayiVeVirgul += c;
                    virgulKullanildi = true;
                }
            }

            // 4. Ayrıştırma ve Formatlama
            if (string.IsNullOrEmpty(sadeceSayiVeVirgul))
            {
                // Giriş boşsa veya sadece geçersiz karakter varsa 0,00 TL göster.
                txt.Text = 0.ToString("N2", new CultureInfo("tr-TR")) + " TL";
            }
            else
            {
                // Türk kültürüne göre (virgül ondalık ayırıcı) ayrıştırmayı dene.
                if (decimal.TryParse(sadeceSayiVeVirgul,
                                     NumberStyles.Currency,
                                     new CultureInfo("tr-TR"),
                                     out decimal sonuc))
                {
                    // Başarılı ayrıştırma: Sayıyı N2 formatında ve " TL" ekleyerek ayarla.
                    // Örnek: "5" -> 5,00 TL
                    // Örnek: "55" -> 55,00 TL
                    // Örnek: "5,5" -> 5,50 TL
                    txt.Text = sonuc.ToString("N2", new CultureInfo("tr-TR")) + " TL";
                }
                else
                {
                    // Ayrıştırma başarısız olursa (nadiren olmalı), 0,00 TL göster
                    txt.Text = 0.ToString("N2", new CultureInfo("tr-TR")) + " TL";
                }
            }

            // 5. İmleç Konumunu Ayarla
            // İmleci virgülün soluna (lira kısmı) konumlandır.
            int pos = txt.Text.LastIndexOf(",");
            if (pos != -1)
            {
                txt.SelectionStart = pos;
            }
            else
            {
                // Eğer virgül yoksa (örneğin 1.000 TL), imleci " TL" kısmının soluna ayarla
                txt.SelectionStart = txt.Text.Length - 3;
            }


            // 6. TextChanged olayını tekrar ata.
            txt.TextChanged += handler;

            // Hesaplamaları tetikle
            ProfitCalculation();
            DebtCalculation();
        }

        private void txtNet_TextChanged(object sender, EventArgs e)
        {
            FormatTL(txtNet, txtNet_TextChanged);
            ProfitCalculation();
            DebtCalculation();
        }

        private void txtBrut_TextChanged(object sender, EventArgs e)
        {
            FormatTL(txtBrut, txtBrut_TextChanged);
            ProfitCalculation();
        }

        private void txtPesin_TextChanged(object sender, EventArgs e)
        {
            FormatTL(txtPesin, txtPesin_TextChanged);
            DebtCalculation();
        }

        // Designer'ın referans verdiği eksik handler'lar:
        private void txtAdet_TextChanged(object sender, EventArgs e)
        {
            ProfitCalculation();
            DebtCalculation();
        }

        private void txtBorc_TextChanged(object sender, EventArgs e)
        {
            // txtBorc readonly; designer referansı olduğu için boş bırakıldı.
        }
    }
}
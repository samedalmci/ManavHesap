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
                while (dr.Read())
                {
                    txtUrunAdi.Items.Add(dr["UrunAdi"].ToString());
                }
                dr.Close();
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
                while (dr.Read())
                {
                    txtMagza.Items.Add(dr["MagazaAdi"].ToString());
                }
                dr.Close();
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
                if (string.IsNullOrWhiteSpace(txtUrunAdi.Text) || string.IsNullOrWhiteSpace(txtAdet.Text) || txtAdet.Text == "0")
                {
                    MessageBox.Show("Lütfen Ürün Adı ve geçerli bir Adet girin.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal adet = int.TryParse(txtAdet.Text, out int a) ? a : 0;
                decimal net = decimal.TryParse(txtNet.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal n) ? n : 0;
                decimal brut = decimal.TryParse(txtBrut.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal b) ? b : 0;
                string karZararText = txtKarZarar.Text.Replace(" TL", "");
                decimal.TryParse(karZararText, NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal karValue);
                decimal pesin = decimal.TryParse(txtPesin.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal p) ? p : 0;
                decimal borc = decimal.TryParse(txtBorc.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal bo) ? bo : 0;

                decimal toplamSatisTutari = adet * net;

                if (pesin > toplamSatisTutari)
                {
                    DialogResult result = MessageBox.Show(
                        $"Girilen Peşinat ({pesin:N2} TL), Toplam Satış Tutarı ({toplamSatisTutari:N2} TL) aşıyor. Kaydı onaylıyor musunuz?",
                        "Fazla Ödeme",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.No)
                        return;
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Satislar (UrunAdi, Magaza, Adet, Net, Brut, Kar, Pesin, Borc) " +
                    "VALUES (@ad, @magza, @adet, @net, @brut, @kar, @pesin, @borc)", conn);

                cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text);
                cmd.Parameters.AddWithValue("@magza", txtMagza.Text);
                cmd.Parameters.AddWithValue("@adet", adet);
                cmd.Parameters.AddWithValue("@net", net);
                cmd.Parameters.AddWithValue("@brut", brut);
                cmd.Parameters.AddWithValue("@kar", karValue);
                cmd.Parameters.AddWithValue("@pesin", pesin);
                cmd.Parameters.AddWithValue("@borc", borc);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Satış başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                SatisUrunler();
            }
        }

        private void ProfitCalculation()
        {
            decimal adet = decimal.TryParse(txtAdet.Text, out decimal a) ? a : 0;
            decimal net = decimal.TryParse(txtNet.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal n) ? n : 0;
            decimal brut = decimal.TryParse(txtBrut.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal b) ? b : 0;

            decimal sonuc = adet * (net - brut);

            txtKarZarar.Text = sonuc.ToString("N2", new CultureInfo("tr-TR")) + " TL";
            txtKarZarar.ForeColor = sonuc < 0 ? Color.Red : Color.Green;
        }

        private void DebtCalculation()
        {
            decimal adet = decimal.TryParse(txtAdet.Text, out decimal a) ? a : 0;
            decimal net = decimal.TryParse(txtNet.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal n) ? n : 0;
            decimal pesin = decimal.TryParse(txtPesin.Text.Replace(" TL", ""), NumberStyles.Currency, new CultureInfo("tr-TR"), out decimal p) ? p : 0;

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
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

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
            // ReadOnly olduğu için manuel giriş yok
        }
    }
}

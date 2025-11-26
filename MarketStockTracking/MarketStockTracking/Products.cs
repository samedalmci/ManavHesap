using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace MarketStockTracking
{
    public partial class Products : Form
    {
        string baglanti = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;Connect Timeout=30;";
        SqlConnection conn;

        public Products()
        {
            InitializeComponent();

            conn = new SqlConnection(baglanti);

            txtUrunCesidi.DropDownStyle = ComboBoxStyle.DropDownList;
            txtUrunCesidi.Items.Clear();
            txtUrunCesidi.Items.Add("Seçiniz...");
            txtUrunCesidi.Items.Add("Sebze");
            txtUrunCesidi.Items.Add("Meyve");
            txtUrunCesidi.SelectedIndex = 0;

            // DİREKT BURADA ÇAĞIR
            try
            {
                ListeleUrunler();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yüklenemedi: " + ex.Message);
            }
        }

        private void Products_Load(object sender, EventArgs e)
        {
            // Bağlantı zaten Constructor'da oluşturuldu
            ListeleUrunler();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            // BOŞ ALAN KONTROLÜ
            if (string.IsNullOrWhiteSpace(txtUrunAdi.Text))
            {
                MessageBox.Show("Lütfen ürün adı girin.");
                return;
            }

            // SEÇİNİZ KONTROLÜ
            if (txtUrunCesidi.SelectedIndex == 0)
            {
                MessageBox.Show("Lütfen ürün çeşidi seçin.");
                return;
            }

            try
            {
                // NULL KONTROLÜ EKLE
                if (conn == null)
                {
                    conn = new SqlConnection(baglanti);
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Urunler (UrunAdi, UrunCesidi, EklenmeTarihi) VALUES (@ad, @cesit, @tarih)", conn);

                cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text.Trim());
                cmd.Parameters.AddWithValue("@cesit", txtUrunCesidi.Text);
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Ürün başarıyla eklendi!");
                    txtUrunAdi.Clear();
                    txtUrunCesidi.SelectedIndex = 0;
                    ListeleUrunler();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ürün eklenirken hata oluştu: " + ex.Message);
            }
            finally
            {
                // NULL KONTROLÜ EKLE
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnEkle_Click(sender, e);
        }

        private void ListeleUrunler()
        {
            try
            {
                // NULL KONTROLÜ EKLE
                if (conn == null)
                {
                    conn = new SqlConnection(baglanti);
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Urunler ORDER BY EklenmeTarihi DESC", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvUrunler.DataSource = dt;

                // Kolon başlıklarını düzenle
                if (dgvUrunler.Columns.Count > 0)
                {
                    if (dgvUrunler.Columns["UrunID"] != null)
                        dgvUrunler.Columns["UrunID"].HeaderText = "ID";
                    if (dgvUrunler.Columns["UrunAdi"] != null)
                        dgvUrunler.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                    if (dgvUrunler.Columns["UrunCesidi"] != null)
                        dgvUrunler.Columns["UrunCesidi"].HeaderText = "Çeşit";
                    if (dgvUrunler.Columns["EklenmeTarihi"] != null)
                        dgvUrunler.Columns["EklenmeTarihi"].HeaderText = "Eklenme Tarihi";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ürünler listelenirken hata: " + ex.Message);
            }
            finally
            {
                // NULL KONTROLÜ EKLE
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void txtUrunAdi_TextChanged(object sender, EventArgs e)
        {
        }

        // Form kapanırken bağlantıyı kapat ve dispose et
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (conn != null)
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Dispose();
            }

            base.OnFormClosing(e);
        }
    }
}
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace MarketStockTracking
{
    public partial class Stores : Form
    {
        DataTable tablo; // Mağaza listesini tutacak tablo
        string baglanti = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;";
        SqlConnection conn; // Nesne tanımı kalıyor

        public Stores()
        {
            InitializeComponent();

            // HATA ÇÖZÜMÜ: conn oluşturma buradan silindi ve Load olayına taşındı.

            TabloHazirla(); // Bu kalabilir, veritabanı işlemi yapmaz
        }

        private void Stores_Load(object sender, EventArgs e)
        {
            // HATA ÇÖZÜMÜ: Bağlantı nesnesini burada oluşturuyoruz
            conn = new SqlConnection(baglanti);

            MagazalariYukle(); // Form açıldığında DB’den verileri çek

            // DataGridView başlıklarını ayarla
            if (dgvMagazalar.Columns.Contains("MagazaAdi"))
            {
                dgvMagazalar.Columns["MagazaAdi"].HeaderText = "Mağaza Adı";
            }
            if (dgvMagazalar.Columns.Contains("Tarih"))
            {
                dgvMagazalar.Columns["Tarih"].HeaderText = "Ekleme Tarihi";
                dgvMagazalar.Columns["Tarih"].DefaultCellStyle.Format = "dd.MM.yyyy"; // Tarih formatı
            }
        }

        private void TabloHazirla()
        {
            tablo = new DataTable();
            tablo.Columns.Add("Tarih");
            tablo.Columns.Add("MagazaAdi");
            dgvMagazalar.DataSource = tablo;
        }

        private void MagazalariYukle()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Magazalar", conn);
                tablo.Clear();
                da.Fill(tablo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı yükleme hatası: " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnEkle_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMagazaAdi.Text))
            {
                MessageBox.Show("Lütfen mağaza adı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Magazalar (Tarih, MagazaAdi) VALUES (@tarih, @magaza)", conn);
                // Tarih parametresini DateTime.Now olarak göndermek daha güvenlidir.
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now);
                cmd.Parameters.AddWithValue("@magaza", txtMagazaAdi.Text);
                cmd.ExecuteNonQuery();

                // DataTable'a da ekle (Doğru DataTable formatında eklenmeli)
                DataRow row = tablo.NewRow();
                row["Tarih"] = DateTime.Now; // Tarih sütununa DateTime nesnesi ekle
                row["MagazaAdi"] = txtMagazaAdi.Text;
                tablo.Rows.Add(row);

                txtMagazaAdi.Clear();
                MessageBox.Show("Mağaza başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
        }
    }
}
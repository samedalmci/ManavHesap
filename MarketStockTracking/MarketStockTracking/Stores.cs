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
        SqlConnection conn;

        public Stores()
        {
            InitializeComponent();
            conn = new SqlConnection(baglanti);
            TabloHazirla();
        }

        private void Stores_Load(object sender, EventArgs e)
        {
            MagazalariYukle(); // Form açıldığında DB’den verileri çek
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
                conn.Open();
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
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Magazalar (Tarih, MagazaAdi) VALUES (@tarih, @magaza)", conn);
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@magaza", txtMagazaAdi.Text);
                cmd.ExecuteNonQuery();

                // DataTable'a da ekle
                DataRow row = tablo.NewRow();
                row["Tarih"] = DateTime.Now.ToString("yyyy-MM-dd");
                row["MagazaAdi"] = txtMagazaAdi.Text;
                tablo.Rows.Add(row);

                txtMagazaAdi.Clear();
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

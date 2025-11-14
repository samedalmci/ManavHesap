using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;


namespace MarketStockTracking
{

    public partial class Products : Form
    {



        string baglanti = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProductStokDB;Integrated Security=True;";
        SqlConnection conn;

        public Products()
        {
            InitializeComponent();

            // BURAYA EKLE
            txtUrunCesidi.DropDownStyle = ComboBoxStyle.DropDownList;
            txtUrunCesidi.Items.Clear();
            txtUrunCesidi.Items.Add("Seçiniz...");
            txtUrunCesidi.Items.Add("Sebze");
            txtUrunCesidi.Items.Add("Meyve");
            txtUrunCesidi.SelectedIndex = 0;

            conn = new SqlConnection(baglanti);
            ListeleUrunler();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            // SEÇİNİZ KONTROLÜ
            if (txtUrunCesidi.SelectedIndex == 0)
            {
                MessageBox.Show("Lütfen ürün çeşidi seçin.");
                return;
            }

            conn.Open();
            SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Urunler (UrunAdi, UrunCesidi, EklenmeTarihi)VALUES(@ad, @cesit, @tarih)", conn);
            cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text);
            cmd.Parameters.AddWithValue("@cesit", txtUrunCesidi.Text);
            cmd.Parameters.AddWithValue("@tarih", DateTime.Now);
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Ürün eklendi!");
            ListeleUrunler();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            btnEkle_Click(sender, e);
        }

        private void ListeleUrunler()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Urunler", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUrunler.DataSource = dt; // DataGridView’e ata
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void txtUrunAdi_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

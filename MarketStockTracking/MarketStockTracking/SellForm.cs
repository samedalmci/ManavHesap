using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;




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
        }

        private void SellForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Satislar (UrunAdi, Magaza, Adet, Net, Brut, Kar, Pesin, Borc) " +
                    "VALUES (@ad, @magza, @adet, @net, @brut, @kar, @pesin, @borc)", conn);

                cmd.Parameters.AddWithValue("@ad", txtUrunAdi.Text);
                cmd.Parameters.AddWithValue("@magza", txtMagza.Text);
                cmd.Parameters.AddWithValue("@adet", int.TryParse(txtAdet.Text, out int adet) ? adet : 0);
                cmd.Parameters.AddWithValue("@net", decimal.TryParse(txtNet.Text, out decimal net) ? net : 0);
                cmd.Parameters.AddWithValue("@brut", decimal.TryParse(txtBrut.Text, out decimal brut) ? brut : 0);
                cmd.Parameters.AddWithValue("@kar", decimal.TryParse(txtKarZarar.Text, out decimal kar) ? kar : 0);
                cmd.Parameters.AddWithValue("@pesin", decimal.TryParse(txtPesin.Text, out decimal pesin) ? pesin : 0);
                cmd.Parameters.AddWithValue("@borc", decimal.TryParse(txtBorc.Text, out decimal borc) ? borc : 0);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Satış eklendi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                conn.Close();
                ProfitCalculation();
                SatisUrunler();
            }
        }



        //FONKSIYONLAR

        private void ProfitCalculation()
        {
            decimal adet = Convert.ToDecimal(txtAdet.Text);
            decimal net = Convert.ToDecimal(txtNet.Text.Replace(" TL", "").Replace(".", "").Replace(",", "."));
            decimal brut = Convert.ToDecimal(txtBrut.Text.Replace(" TL", "").Replace(".", "").Replace(",", "."));

            decimal sonuc = adet * (net - brut);
            txtKarZarar.Text = sonuc.ToString("N2") + " TL";

            txtKarZarar.ForeColor = sonuc < 0 ? Color.Red : Color.Green;           

        }

   


        private void SatisUrunler()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Satislar", conn);
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


        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void txtNet_TextChanged(object sender, EventArgs e)
        {
            if (txtNet.Text == "") return;

            string temiz = txtNet.Text.Replace(".", "").Replace(",", "").Replace(" TL", "");

            if (decimal.TryParse(temiz, out decimal deger))
            {
                txtNet.TextChanged -= txtNet_TextChanged;
                txtNet.Text = string.Format("{0:N2} TL", deger / 100); // 2000 → 20,00 TL
                txtNet.SelectionStart = txtNet.Text.Length - 3; // imleci TL’den önce konumlandır
                txtNet.TextChanged += txtNet_TextChanged;
            }
        }

        private void txtBrut_TextChanged(object sender, EventArgs e)
        {
            if (txtBrut.Text == "") return;

            string temiz = txtBrut.Text.Replace(".", "").Replace(",", "").Replace(" TL", "");

            if (decimal.TryParse(temiz, out decimal deger))
            {
                txtBrut.TextChanged -= txtBrut_TextChanged;
                txtBrut.Text = string.Format("{0:N2} TL", deger / 100);
                txtBrut.SelectionStart = txtBrut.Text.Length - 3;
                txtBrut.TextChanged += txtBrut_TextChanged;
            }
        }
    }
}

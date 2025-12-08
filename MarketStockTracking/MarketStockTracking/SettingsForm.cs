using System;
using System.Windows.Forms;

namespace MarketStockTracking
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            AyarlariYukle();
        }

        private void AyarlariYukle()
        {
            // Mevcut ayarları yükle
            txtIsletmeAdi.Text = DatabaseHelper.GetSetting("IsletmeAdi", "");
            txtAdres.Text = DatabaseHelper.GetSetting("Adres", "");
            txtTelefon.Text = DatabaseHelper.GetSetting("Telefon", "");
            txtSehir.Text = DatabaseHelper.GetSetting("Sehir", "");
            txtKasiyer.Text = DatabaseHelper.GetSetting("Kasiyer", "");
            txtYaziciAdi.Text = DatabaseHelper.GetSetting("YaziciAdi", "");
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            // Ayarları kaydet
            DatabaseHelper.SetSetting("IsletmeAdi", txtIsletmeAdi.Text.Trim());
            DatabaseHelper.SetSetting("Adres", txtAdres.Text.Trim());
            DatabaseHelper.SetSetting("Telefon", txtTelefon.Text.Trim());
            DatabaseHelper.SetSetting("Sehir", txtSehir.Text.Trim());
            DatabaseHelper.SetSetting("Kasiyer", txtKasiyer.Text.Trim());
            DatabaseHelper.SetSetting("YaziciAdi", txtYaziciAdi.Text.Trim());

            MessageBox.Show("Ayarlar kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
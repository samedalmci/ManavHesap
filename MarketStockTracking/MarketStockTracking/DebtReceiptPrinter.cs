using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using MarketStockTracking.Models;
using Microsoft.Data.Sqlite;

namespace MarketStockTracking
{
    public class DebtReceiptPrinter
    {
        private readonly CultureInfo trCulture = new CultureInfo("tr-TR");

        public string IsletmeAdi { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string Sehir { get; set; }
        public string YaziciAdi { get; set; }
        public string Kasiyer { get; set; }

        public DateTime OdemeTarihi { get; set; }
        public List<DebtPaymentInfo> Odemeler { get; set; }
        public decimal ToplamOdenen { get; set; }
        public decimal ToplamSecilenBorc { get; set; }
        public string MagazaAdi { get; set; }

        // Mağaza genel bilgileri
        public decimal MagazaToplamTutar { get; set; }
        public decimal MagazaToplamOdenen { get; set; }
        public decimal MagazaKalanBorc { get; set; }

        public DebtReceiptPrinter(List<DebtPaymentInfo> odemeler)
        {
            // Ayarları DB'den yükle
            this.IsletmeAdi = DatabaseHelper.GetSetting("IsletmeAdi", "İşletme Adı");
            this.Adres = DatabaseHelper.GetSetting("Adres", "Adres");
            this.Telefon = DatabaseHelper.GetSetting("Telefon", "Telefon");
            this.Sehir = DatabaseHelper.GetSetting("Sehir", "Şehir");
            this.YaziciAdi = DatabaseHelper.GetSetting("YaziciAdi", "Microsoft Print to PDF");
            this.Kasiyer = DatabaseHelper.GetSetting("Kasiyer", "Kasiyer");

            this.OdemeTarihi = DateTime.Now;
            this.Odemeler = odemeler;

            // Toplamları hesapla
            decimal toplamOdenen = 0;
            decimal toplamSecilenBorc = 0;

            foreach (var odeme in odemeler)
            {
                toplamOdenen += odeme.OdenenMiktar;
                toplamSecilenBorc += odeme.EskiBorc;
            }

            this.ToplamOdenen = toplamOdenen;
            this.ToplamSecilenBorc = toplamSecilenBorc;

            // Mağaza adı - ilk ödemeden al
            if (odemeler.Count > 0)
            {
                this.MagazaAdi = odemeler[0].MagazaAdi;
            }

            // Mağaza genel bilgilerini DB'den çek
            if (!string.IsNullOrEmpty(this.MagazaAdi))
            {
                using (var conn = new SqliteConnection(DatabaseHelper.ConnectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = @"SELECT 
                                COALESCE(SUM(Quantity * NetPrice), 0) AS ToplamTutar,
                                COALESCE(SUM(CashPaid), 0) AS ToplamOdenen,
                                COALESCE(SUM(Debt), 0) AS KalanBorc
                            FROM Sales 
                            WHERE StoreName = @magaza";
                    cmd.Parameters.AddWithValue("@magaza", this.MagazaAdi);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            this.MagazaToplamTutar = reader.GetDecimal(0);
                            this.MagazaToplamOdenen = reader.GetDecimal(1);
                            this.MagazaKalanBorc = reader.GetDecimal(2);
                        }
                    }
                }
            }
        }

        public void Bas()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

            if (!string.IsNullOrWhiteSpace(YaziciAdi))
            {
                try
                {
                    pd.PrinterSettings.PrinterName = YaziciAdi;
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show($"'{YaziciAdi}' adında bir yazıcı bulunamadı.", "Yazıcı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }

            // Kağıt yüksekliğini içeriğe göre hesapla
            int sabitSatirlar = 28;
            int odemeSatirlari = Odemeler.Count * 2; // Her ürün 2 satır
            int toplamSatir = sabitSatirlar + odemeSatirlari;
            int altBosluk = 50;
            int kagitYuksekligi = (toplamSatir * 20) + altBosluk;

            pd.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 315, kagitYuksekligi);
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

            pd.Print();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int y = 0;
            int lineHeight = 18;

            Font fontBaslik = new Font("Arial", 10, FontStyle.Bold);
            Font fontAdres = new Font("Arial", 8, FontStyle.Regular);
            Font fontNormal = new Font("Courier New", 9, FontStyle.Regular);
            Font fontUrun = new Font("Courier New", 9, FontStyle.Bold);
            Font fontToplam = new Font("Courier New", 10, FontStyle.Bold);

            StringFormat rightAlign = new StringFormat();
            rightAlign.Alignment = StringAlignment.Far;

            int leftMargin = 5;
            int rightMargin = e.PageBounds.Width - 5;

            // 1. İŞLETME BİLGİLERİ
            SizeF size = g.MeasureString(IsletmeAdi, fontBaslik);
            float startX = (e.PageBounds.Width / 2) - (size.Width / 2);
            g.DrawString(IsletmeAdi, fontBaslik, Brushes.Black, startX, y);
            y += (int)size.Height;

            size = g.MeasureString(Adres, fontAdres);
            startX = (e.PageBounds.Width / 2) - (size.Width / 2);
            g.DrawString(Adres, fontAdres, Brushes.Black, startX, y);
            y += (int)size.Height;

            size = g.MeasureString(Telefon, fontAdres);
            startX = (e.PageBounds.Width / 2) - (size.Width / 2);
            g.DrawString(Telefon, fontAdres, Brushes.Black, startX, y);
            y += (int)size.Height;

            size = g.MeasureString(Sehir, fontAdres);
            startX = (e.PageBounds.Width / 2) - (size.Width / 2);
            g.DrawString(Sehir, fontAdres, Brushes.Black, startX, y);
            y += lineHeight;

            // 2. BORÇ ÖDEME BAŞLIĞI
            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            size = g.MeasureString("BORÇ ÖDEME FİŞİ", fontToplam);
            startX = (e.PageBounds.Width / 2) - (size.Width / 2);
            g.DrawString("BORÇ ÖDEME FİŞİ", fontToplam, Brushes.Black, startX, y);
            y += lineHeight;

            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            g.DrawString($"TARİH : {OdemeTarihi.ToString("dd.MM.yyyy")}", fontNormal, Brushes.Black, leftMargin, y);
            g.DrawString($"SAAT : {OdemeTarihi.ToString("HH:mm")}", fontNormal, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            g.DrawString($"KASİYER : {Kasiyer}", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            // 3. SEÇİLİ ÜRÜNLER (Tek tek listele)
            size = g.MeasureString("SEÇİLİ ÜRÜNLER", fontUrun);
            startX = (e.PageBounds.Width / 2) - (size.Width / 2);
            g.DrawString("SEÇİLİ ÜRÜNLER", fontUrun, Brushes.Black, startX, y);
            y += lineHeight;

            foreach (var odeme in Odemeler)
            {
                g.DrawString(odeme.UrunAdi, fontUrun, Brushes.Black, leftMargin, y);
                g.DrawString(odeme.EskiBorc.ToString("N2", trCulture), fontNormal, Brushes.Black, rightMargin, y, rightAlign);
                y += lineHeight;

                g.DrawString($"  Ödenen: {odeme.OdenenMiktar.ToString("N2", trCulture)}", fontNormal, Brushes.Black, leftMargin, y);
                g.DrawString($"Kalan: {odeme.YeniBorc.ToString("N2", trCulture)}", fontNormal, Brushes.Black, rightMargin, y, rightAlign);
                y += lineHeight;
            }

            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            // 4. SEÇİLİ ÜRÜNLER TOPLAMI
            g.DrawString("SEÇİLİ BORÇ", fontNormal, Brushes.Black, leftMargin, y);
            g.DrawString(ToplamSecilenBorc.ToString("N2", trCulture), fontNormal, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            g.DrawString("BU ÖDEME", fontToplam, Brushes.Black, leftMargin, y);
            g.DrawString(ToplamOdenen.ToString("N2", trCulture), fontToplam, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            decimal kalanSeciliBorc = ToplamSecilenBorc - ToplamOdenen;
            g.DrawString("KALAN SEÇİLİ BORÇ", fontNormal, Brushes.Black, leftMargin, y);
            g.DrawString(kalanSeciliBorc.ToString("N2", trCulture), fontNormal, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            // 5. MAĞAZA BİLGİLERİ
            // 5. MAĞAZA BİLGİLERİ
            size = g.MeasureString("MAĞAZA BİLGİLERİ", fontToplam);
            startX = (e.PageBounds.Width / 2) - (size.Width / 2);
            g.DrawString("MAĞAZA BİLGİLERİ", fontToplam, Brushes.Black, startX, y);
            y += lineHeight;

            g.DrawString($"Mağaza: {MagazaAdi}", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            g.DrawString("Toplam Borç:", fontNormal, Brushes.Black, leftMargin, y);
            g.DrawString(MagazaToplamTutar.ToString("N2", trCulture), fontNormal, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            g.DrawString("Toplam Ödenen:", fontNormal, Brushes.Black, leftMargin, y);
            g.DrawString(MagazaToplamOdenen.ToString("N2", trCulture), fontNormal, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            g.DrawString("Kalan Borç:", fontToplam, Brushes.Black, leftMargin, y);
            g.DrawString(MagazaKalanBorc.ToString("N2", trCulture), fontToplam, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            // 6. KDV NOTU
            size = g.MeasureString("BORÇ ÖDEME MAKBUZU", fontNormal);
            startX = (e.PageBounds.Width / 2) - (size.Width / 2);
            g.DrawString("BORÇ ÖDEME MAKBUZU", fontNormal, Brushes.Black, startX, y);
            y += lineHeight;

            // Alt boşluk
            y += 30;
            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);


            e.HasMorePages = false;
        }
    }
}
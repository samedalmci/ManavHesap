using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using MarketStockTracking.Models;

namespace MarketStockTracking
{
    public class ReceiptPrinter
    {
        // Türk kültür ayarlarını kullanıyoruz
        private readonly CultureInfo trCulture = new CultureInfo("tr-TR");

        // Fişte kullanılacak veriler (Tek bir satış kaydı için)   
     
       
        // Satışa ait temel veriler
        public DateTime SatisTarihi { get; set; }
        public int SatisNo { get; set; } = 1; // Eğer veritabanından ID alınıyorsa buraya atanmalı
     


        public string IsletmeAdi { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string Sehir { get; set; }
        public string YaziciAdi { get; set; }
        public string Kasiyer { get; set; }

        public List<Sale> Satislar { get; set; }
        public decimal ToplamTutar { get; set; }
        public string MagzaAdi { get; set; }
        public decimal AlinanPara { get; set; }
        public decimal ParaUstu { get; set; }
        public decimal Borc { get; set; }



        public ReceiptPrinter(BindingList<Sale> satislar)
        {
            // Ayarları DB'den yükle
            this.IsletmeAdi = DatabaseHelper.GetSetting("IsletmeAdi", "İşletme Adı");
            this.Adres = DatabaseHelper.GetSetting("Adres", "Adres");
            this.Telefon = DatabaseHelper.GetSetting("Telefon", "Telefon");
            this.Sehir = DatabaseHelper.GetSetting("Sehir", "Şehir");
            this.YaziciAdi = DatabaseHelper.GetSetting("YaziciAdi", "Microsoft Print to PDF");
            this.Kasiyer = DatabaseHelper.GetSetting("Kasiyer", "Kasiyer");


            this.SatisTarihi = DateTime.Now;
            this.Satislar = new List<Sale>(satislar);


            // Toplamları hesapla
            decimal toplamTutar = 0;
            decimal toplamPesinat = 0;
            decimal toplamBorc = 0; 
            
            foreach (Sale sale in satislar)
            {
                toplamTutar += sale.Quantity * sale.NetPrice;
                toplamPesinat += sale.CashPaid;
                toplamBorc += sale.Debt;
            }

            this.ToplamTutar = toplamTutar;
            this.AlinanPara = toplamPesinat;
            this.Borc = toplamBorc;

            // Mağaza adı - ilk üründen al
            if (satislar.Count > 0)
            {
                this.MagzaAdi = satislar[0].StoreName;
            }

            // Para üstü hesaplama
            if (toplamPesinat > this.ToplamTutar && this.Borc == 0)
            {
                this.ParaUstu = toplamPesinat - this.ToplamTutar;
            }
            else
            {
                this.ParaUstu = 0;
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
            int sabitSatirlar = 18; // İşletme bilgileri, tarih, toplam vs.
            int urunSatirlari = Satislar.Count * 2; // Her ürün 2 satır
            int toplamSatir = sabitSatirlar + urunSatirlari;
            int kagitYuksekligi = toplamSatir * 20; // Her satır ~20 piksel

            pd.DefaultPageSettings.PaperSize = new PaperSize("Receipt", 315, kagitYuksekligi);
            pd.DefaultPageSettings.Margins = new Margins(5, 5, 5, 5);

            pd.Print();
        }



        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Genellikle 80mm yazıcılar 3.125 inç genişliğindedir (~79.3mm). 
            // 1 inç = 100 piksel varsayarak, bu yazıcılar için maksimum genişlik 300-310 civarıdır. 
            // Biz burada sadece başlangıç X pozisyonu (0) ve Y pozisyonunu kullanacağız.
            Graphics g = e.Graphics;
            int y = 5;
            int lineHeight = 18; // Satırlar arası boşluk

            // Fiş fontları
            Font fontBaslik = new Font("Arial", 10, FontStyle.Bold);
            Font fontAdres = new Font("Arial", 8, FontStyle.Regular);
            Font fontNormal = new Font("Courier New", 9, FontStyle.Regular);
            Font fontUrun = new Font("Courier New", 9, FontStyle.Bold);
            Font fontToplam = new Font("Courier New", 10, FontStyle.Bold);

            // Hizalama formatı
            StringFormat rightAlign = new StringFormat();
            rightAlign.Alignment = StringAlignment.Far; // Sağa hizalama

            // ----------------------------------------------------
            // 1. İŞLETME BİLGİLERİ (Ortalanmış)
            // ----------------------------------------------------
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

            // ----------------------------------------------------
            // 2. SATIŞ BİLGİLERİ (İki Yana Yaslı)
            // ----------------------------------------------------
            int leftMargin = 5;
            int rightMargin = e.PageBounds.Width - 5;
            int midPoint = e.PageBounds.Width / 2;

            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            g.DrawString($"TARİH : {SatisTarihi.ToString("dd.MM.yyyy")}", fontNormal, Brushes.Black, leftMargin, y);
            g.DrawString($"SAAT : {SatisTarihi.ToString("HH:mm")}", fontNormal, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            g.DrawString($"SATIŞ NO : {SatisNo}", fontNormal, Brushes.Black, leftMargin, y);
            g.DrawString($"SATIŞ : NAKİT", fontNormal, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            g.DrawString($"KASİYER : {Kasiyer}", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            // ----------------------------------------------------
            // 3. ÜRÜNLER (Görseldeki Formata Yakın)
            // ----------------------------------------------------

            foreach (Sale sale in Satislar)
            {
                decimal urunToplam = sale.Quantity * sale.NetPrice;
                string urunMiktar = $"{sale.Quantity.ToString("N3", trCulture).TrimEnd('0', ',')} X {sale.NetPrice.ToString("N2", trCulture)}";

                g.DrawString(sale.ProductName, fontUrun, Brushes.Black, leftMargin, y);
                g.DrawString(urunToplam.ToString("N2", trCulture), fontUrun, Brushes.Black, rightMargin, y, rightAlign);
                y += lineHeight;

                g.DrawString(urunMiktar, fontNormal, Brushes.Black, leftMargin + 5, y);
                y += lineHeight;
            }


            // ----------------------------------------------------
            // 4. TOPLAM VE ÖDEME BİLGİLERİ
            // ----------------------------------------------------

            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            // Genel Toplam
            g.DrawString("GENEL TOPLAM", fontToplam, Brushes.Black, leftMargin, y);
            g.DrawString(ToplamTutar.ToString("N2", trCulture), fontToplam, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight * 2;

            // Alınan Para / Peşinat
            g.DrawString("ALINAN PARA", fontNormal, Brushes.Black, leftMargin, y);
            g.DrawString(AlinanPara.ToString("N2", trCulture), fontNormal, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            // Para Üstü
            g.DrawString("PARA ÜSTÜ", fontNormal, Brushes.Black, leftMargin, y);
            g.DrawString(ParaUstu.ToString("N2", trCulture), fontNormal, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            // Borç (Eğer Borç varsa göster)
            if (Borc > 0)
            {
                g.DrawString("Kalan Borç", fontToplam, Brushes.Black, leftMargin, y);
                g.DrawString(Borc.ToString("N2", trCulture), fontToplam, Brushes.Black, rightMargin, y, rightAlign);
                y += lineHeight;
            }

            g.DrawString("-----------------------------------------", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            // 5. SON NOT
            size = g.MeasureString("KDV FİŞİ DEĞİLDİR", fontNormal);
            startX = (e.PageBounds.Width / 2) - (size.Width / 2);
            g.DrawString("KDV FİŞİ DEĞİLDİR", fontNormal, Brushes.Black, startX, y);
            y += lineHeight;

            // 6. Mağza İsmi
            g.DrawString($"Mağza Adı : {MagzaAdi}", fontNormal, Brushes.Black, leftMargin, y);
            y += lineHeight;

            e.HasMorePages = false; // Yazdırmanın bittiğini belirtir
        }
    }
}
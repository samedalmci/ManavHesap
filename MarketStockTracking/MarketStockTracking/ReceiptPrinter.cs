using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;

namespace MarketStockTracking
{
    public class ReceiptPrinter
    {
        // Türk kültür ayarlarını kullanıyoruz
        private readonly CultureInfo trCulture = new CultureInfo("tr-TR");

        // Fişte kullanılacak veriler (Tek bir satış kaydı için)
        public string IsletmeAdi { get; set; } = "X Manav";
        public string Adres { get; set; } = "Dalyan, Atatürk Blv. 29/g, 48600 Ortaca/Muğla";
        public string Telefon { get; set; } = "0252 XXX XX XX";
        public string Sehir { get; set; } = "Muğla";
        public string YaziciAdi { get; set; } = "XPRINTER XP-Q805K"; // Varsayılan Yazıcınızın Windows'taki Adı

        // Satışa ait temel veriler
        public DateTime SatisTarihi { get; set; }
        public int SatisNo { get; set; } = 1; // Eğer veritabanından ID alınıyorsa buraya atanmalı
        public string Kasiyer { get; set; } = "Mert Kıydan"; // Varsayılan Kasiyer
        //public string UrunBarkod { get; set; }
        public string UrunAdi { get; set; }
        public string MagzaAdi { get; set; }
        public decimal Adet { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal ToplamTutar { get; set; }
        public decimal AlinanPara { get; set; }
        public decimal ParaUstu { get; set; }
        public decimal Borc { get; set; }



        public ReceiptPrinter(string urunAdi, decimal adet, decimal birimFiyat, decimal pesin, decimal borc, string magzaAdi)
        {
            this.SatisTarihi = DateTime.Now;
            this.UrunAdi = urunAdi;
            this.Adet = adet;
            this.MagzaAdi = magzaAdi;
            this.BirimFiyat = birimFiyat;
            this.ToplamTutar = adet * birimFiyat;
            this.AlinanPara = pesin;
            this.Borc = borc;

            // Eğer peşin ödenen miktar toplam tutardan fazlaysa, para üstü hesaplanır.
            if (pesin > this.ToplamTutar && this.Borc == 0)
            {
                this.ParaUstu = pesin - this.ToplamTutar;
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

            // Geliştirme/Test aşamasında Microsoft Print to PDF kullanmak için:
            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            // Gerçek Yazıcı Kullanımı için (Yukarıdaki satırı yorum satırı yapın):
            /*
            try
            {
                pd.PrinterSettings.PrinterName = YaziciAdi;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show($"'{YaziciAdi}' adında bir yazıcı bulunamadı. Varsayılan yazıcıya gönderiliyor.", "Yazıcı Hatası", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            */

            // **80mm Fiş (3.15 inç x 11.69 inç (uzun)) için kağıt boyutunu ayarla**
            // Genişlik: 3.15 inç * 100 DPI = ~315 (Windows Forms varsayılan birimi inç/100)
            // Yükseklik: Çok uzun yapıyoruz ki tek sayfaya sığsın
            pd.DefaultPageSettings.PaperSize = new PaperSize("80mm Receipt", 315, 1169); // Genişlik: 80mm
            pd.DefaultPageSettings.Margins = new Margins(5, 5, 5, 5); // Kenar boşluklarını azalt

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

            // Örnek Fişteki Ürün Satırını Oluşturma
            // UrunAdi + (Adet X BirimFiyat)
            string urunMiktar = $"{Adet.ToString("N3", trCulture).TrimEnd('0', ',')} X {BirimFiyat.ToString("N2", trCulture)}";
            string urunSatiri = $"{UrunAdi}";

            g.DrawString(urunSatiri, fontUrun, Brushes.Black, leftMargin, y);
            g.DrawString(ToplamTutar.ToString("N2", trCulture), fontUrun, Brushes.Black, rightMargin, y, rightAlign);
            y += lineHeight;

            g.DrawString(urunMiktar, fontNormal, Brushes.Black, leftMargin + 5, y); // Hafif girinti
            y += lineHeight;


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
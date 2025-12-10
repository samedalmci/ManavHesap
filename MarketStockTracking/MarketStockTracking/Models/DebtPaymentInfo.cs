namespace MarketStockTracking.Models
{
    public class DebtPaymentInfo
    {
        public string UrunAdi { get; set; }
        public string MagazaAdi { get; set; }
        public decimal EskiBorc { get; set; }
        public decimal OdenenMiktar { get; set; }
        public decimal YeniBorc { get; set; }
    }
}
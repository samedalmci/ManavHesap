namespace MarketStockTracking.Models
{
    public class Sale
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string StoreName { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetPrice { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal Profit { get; set; }
        public decimal CashPaid { get; set; }
        public decimal Debt { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

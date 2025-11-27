using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketStockTracking.Models
{
    public class Product
    {
        public int UrunID { get; set; }
        public string UrunAdi { get; set; }
        public string UrunCesidi { get; set; }
        public DateTime EklenmeTarihi { get; set; }
    }
}

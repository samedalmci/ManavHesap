using System.Collections.Generic;
using MarketStockTracking.Models;

namespace MarketStockTracking.Repositories
{
    public interface ISalesRepository
    {
        List<Sale> GetAll();
        int Add(Sale sale);

    }
}

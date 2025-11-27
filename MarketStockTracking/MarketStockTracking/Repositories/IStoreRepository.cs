using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketStockTracking.Models;

namespace MarketStockTracking.Repositories
{
    public interface IStoreRepository
    {
        List<Store> GetAll();


    }
}

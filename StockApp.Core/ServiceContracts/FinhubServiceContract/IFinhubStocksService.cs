using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.ServiceContracts.FinhubServiceContract
{
    public interface IFinhubStocksService
    {
        Task<List<Dictionary<string, string>>?> GetStocks();
    }
}

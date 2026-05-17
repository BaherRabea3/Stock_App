using StockApp.Core.Domain.RepositoryContracts;
using StockApp.Core.Exceptions;
using StockApp.Core.ServiceContracts.FinhubServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Services.FinhubService
{
    public class FinhubStockPriceQouteService : IFinhubStockPriceQouteService
    {
        private readonly IFinhubRepository _finhubRepo;

        public FinhubStockPriceQouteService(IFinhubRepository finhubRepo)
        {
            _finhubRepo = finhubRepo;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            try
            {
                return await _finhubRepo.GetCompanyPriceQoute(stockSymbol);
            }
            catch
            {
                throw new FinhubException("Failed to connect to finhub/api server");
            }
        }
    }
}

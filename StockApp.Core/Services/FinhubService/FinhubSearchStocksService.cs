using StockApp.Core.Domain.RepositoryContracts;
using StockApp.Core.Exceptions;
using StockApp.Core.ServiceContracts.FinhubServiceContract;

namespace StockApp.Core.Services.FinhubService
{
    public class FinhubSearchStocksService : IFinhubSearchStocksService
    {
        private readonly IFinhubRepository _finhubRepo;

        public FinhubSearchStocksService(IFinhubRepository finhubRepo)
        {
            _finhubRepo = finhubRepo;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string StockSymbolToSearch)
        {
            try
            {
                return await _finhubRepo.SearchStocks(StockSymbolToSearch);
            }
            catch
            {
                throw new FinhubException("Failed to connect to finhub/api server");
            }
        }
    }
}

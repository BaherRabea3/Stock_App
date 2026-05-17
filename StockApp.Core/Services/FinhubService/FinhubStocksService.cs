using StockApp.Core.Domain.RepositoryContracts;
using StockApp.Core.Exceptions;
using StockApp.Core.ServiceContracts.FinhubServiceContract;

namespace StockApp.Core.Services.FinhubService
{
    public class FinhubStocksService : IFinhubStocksService
    {
        private readonly IFinhubRepository _finhubRepo;

        public FinhubStocksService(IFinhubRepository finhubRepo)
        {
            _finhubRepo = finhubRepo;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            try
            {
                return await _finhubRepo.GetStocks();
            }
            catch
            {
                throw new FinhubException("Failed to connect to finhub/api server");
            }
        }
    }
}

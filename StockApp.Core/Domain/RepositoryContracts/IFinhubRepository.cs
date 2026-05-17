
namespace StockApp.Core.Domain.RepositoryContracts
{
    public interface IFinhubRepository
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        Task<Dictionary<string, object>?> GetCompanyPriceQoute(string stockSymbol);
        Task<List<Dictionary<string, string>>?> GetStocks();
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}

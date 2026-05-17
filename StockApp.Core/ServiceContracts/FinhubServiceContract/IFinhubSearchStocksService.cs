namespace StockApp.Core.ServiceContracts.FinhubServiceContract
{
    public interface IFinhubSearchStocksService
    {
        Task<Dictionary<string, object>?>SearchStocks(string StockSymbolToSearch);
    }
}

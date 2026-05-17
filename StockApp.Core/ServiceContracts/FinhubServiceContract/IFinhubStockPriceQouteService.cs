namespace StockApp.Core.ServiceContracts.FinhubServiceContract
{
    public interface IFinhubStockPriceQouteService
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}

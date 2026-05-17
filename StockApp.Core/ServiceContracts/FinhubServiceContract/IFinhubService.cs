namespace StockApp.Core.ServiceContracts.FinhubServiceContract
{
    public interface IFinhubCompanyProfileService
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string StockSymbol);
    }
}

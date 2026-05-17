using Microsoft.Extensions.Configuration;
using StockApp.Core.Domain.RepositoryContracts;
using StockApp.Core.Exceptions;
using StockApp.Core.ServiceContracts.FinhubServiceContract;
using System.Text.Json;

namespace StockApp.Core.Services.FinhubService
{
    public class FinhubCompanyProfileService : IFinhubCompanyProfileService
    {
        private readonly IFinhubRepository _finhubRepo;
        public FinhubCompanyProfileService(IFinhubRepository finhubRepo)
        {
            _finhubRepo = finhubRepo;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string StockSymbol)
        {
            try
            {
                return await _finhubRepo.GetCompanyProfile(StockSymbol);
            }
            catch
            {
                throw new FinhubException("Failed to connect to finnhub/api server");
            }
        }

    }
}

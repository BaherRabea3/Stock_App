using Microsoft.Extensions.Configuration;
using StockApp.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockApp.Infrastructure.Repositories
{
    public class FinhubRepository : IFinhubRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public FinhubRepository(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, object>?> GetCompanyPriceQoute(string stockSymbol)
        {
            var responseObj = await GetDataFromFinHubAsync($" https://finnhub.io/api/v1/quote?symbol={stockSymbol}");

            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(responseObj);

            if (response == null)
                throw new InvalidOperationException("No response from finhub server");

            if (response.ContainsKey("Error"))
                throw new InvalidOperationException(response["Error"].ToString());

            return response;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            var responseObj = await GetDataFromFinHubAsync($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}");

            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(responseObj);

            if (response == null)
                throw new InvalidOperationException("No response from finhub server");

            if (response.ContainsKey("Error"))
                throw new InvalidOperationException(response["Error"].ToString());

            return response;

        }
        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var responseObj = await GetDataFromFinHubAsync($"https://finnhub.io/api/v1/stock/symbol?exchange=US");

            var response = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseObj);

            if (response == null)
                throw new InvalidOperationException("No response from finhub server");

            var Errors = response.Where(r => r.ContainsKey("Error"));
            if (Errors.Any())
            {
                var ErrorMsg = string.Join(" | ", Errors.Select(e => e["Error"]));
                throw new InvalidOperationException(ErrorMsg);
            }
            return response;
        }
        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            var responseObj = await GetDataFromFinHubAsync($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}");

            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(responseObj);

            if (response == null)
                throw new InvalidOperationException("No response from finhub server");

            if (response.ContainsKey("Error"))
                throw new InvalidOperationException(response["Error"].ToString());
            return response;
        }
        private async Task<string> GetDataFromFinHubAsync(string url)
        {
            using (var HttpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Get,
                    new Uri($"{url}&token={_configuration["FinhubToken"]}")
                );

                var ResponseMassage = await HttpClient.SendAsync(httpRequestMessage);

                var ResponseStream = await ResponseMassage.Content.ReadAsStreamAsync();

                StreamReader stream = new StreamReader(ResponseStream);
                string responseObject = await stream.ReadToEndAsync();

                return responseObject;
            }
        }
    }
}

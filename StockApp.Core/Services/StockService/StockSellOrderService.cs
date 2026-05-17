using StockApp.Core.Domain.Entities;
using StockApp.Core.Domain.RepositoryContracts;
using StockApp.Core.DTOs.SellOrder;
using StockApp.Core.Helpers;
using StockApp.Core.ServiceContracts.StockServiceContract;
using StockApp.Core.Services.FinhubService;
using System.Runtime.CompilerServices;

namespace StockApp.Core.Services.StockService
{
    public class StockSellOrderService : IStockSellOrderService
    {
        private readonly IStocksRepository _stocksRepo;

        public StockSellOrderService(IStocksRepository stocksRepo)
        {
            _stocksRepo = stocksRepo;
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest sellOrderRequest)
        {
            if(sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            var NewsellOrder = new SellOrder()
            {
                SellOrderId = Guid.NewGuid(),
                StockSymbol = sellOrderRequest.StockSymbol,
                StockName = sellOrderRequest.StockName,
                DateAndTimeOfOrder = sellOrderRequest.DateAndTimeOfOrder,
                Quantity = sellOrderRequest.Quantity,
                Price = sellOrderRequest.Price,
            };

            SellOrder sellOrder = await _stocksRepo.CreateSellOrder(NewsellOrder);

            return MapToSellOrderResponse(sellOrder);
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellOrderList = await _stocksRepo.GetSellOrders();
            return sellOrderList.Select(x => MapToSellOrderResponse(x)).ToList();
        }

        private static SellOrderResponse MapToSellOrderResponse(SellOrder sellOrder) => new SellOrderResponse()
        {
            SellOrderId = sellOrder.SellOrderId,
            StockName = sellOrder.StockName,
            StockSymbol = sellOrder.StockSymbol,
            DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
            Price = sellOrder.Price,
            Quantity = sellOrder.Quantity,
            TradeAmount = sellOrder.Price * sellOrder.Quantity
        };

    }
}

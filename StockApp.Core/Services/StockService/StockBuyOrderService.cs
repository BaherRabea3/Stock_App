using StockApp.Core.Domain.Entities;
using StockApp.Core.Domain.RepositoryContracts;
using StockApp.Core.DTOs.BuyOrder;
using StockApp.Core.Helpers;
using StockApp.Core.ServiceContracts.StockServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Services.StockService
{
    public class StockBuyOrderService : IStockBuyOrderService
    {
        private readonly IStocksRepository _stocksRepo;

        public StockBuyOrderService(IStocksRepository stockRepo)
        {
            _stocksRepo = stockRepo;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            var NewbuyOrder = new BuyOrder
            {
                BuyOrderId = Guid.NewGuid(),
                DateAndTimeOfOrder = buyOrderRequest.DateAndTimeOfOrder,
                Price = buyOrderRequest.Price,
                Quantity = buyOrderRequest.Quantity,
                StockName = buyOrderRequest.StockName,
                StockSymbol = buyOrderRequest.StockSymbol
            };

            var buyOrder = await _stocksRepo.CreateBuyOrder(NewbuyOrder);

            return MapToBuyOrderResponse(buyOrder);
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var BuyOrderList = await _stocksRepo.GetBuyOrders();

            return BuyOrderList.Select(x => MapToBuyOrderResponse(x)).ToList();
        }

        private static BuyOrderResponse MapToBuyOrderResponse(BuyOrder buyOrder) => new BuyOrderResponse()
        {
            BuyOrderId = buyOrder.BuyOrderId,
            StockName = buyOrder.StockName,
            StockSymbol = buyOrder.StockSymbol,
            DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
            Price = buyOrder.Price,
            Quantity = buyOrder.Quantity,
            TradeAmount = buyOrder.Price * buyOrder.Quantity
        };
    }
}

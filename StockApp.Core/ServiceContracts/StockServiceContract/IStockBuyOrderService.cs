using StockApp.Core.DTOs.BuyOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.ServiceContracts.StockServiceContract
{
    public interface IStockBuyOrderService
    {
        Task<List<BuyOrderResponse>> GetBuyOrders();
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrder);
    }
}

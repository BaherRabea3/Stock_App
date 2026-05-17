using StockApp.Core.DTOs.SellOrder;

namespace StockApp.Core.ServiceContracts.StockServiceContract
{
    public interface IStockSellOrderService
    {
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest sellOrder);
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}

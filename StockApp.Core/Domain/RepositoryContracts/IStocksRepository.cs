using StockApp.Core.Domain.Entities;

namespace StockApp.Core.Domain.RepositoryContracts
{
    public interface IStocksRepository
    {
        Task<List<BuyOrder>> GetBuyOrders();
        Task<List<SellOrder>> GetSellOrders();
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);
    }
}

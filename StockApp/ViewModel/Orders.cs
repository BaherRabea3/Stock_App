using StockApp.Core.DTOs.BuyOrder;
using StockApp.Core.DTOs.SellOrder;

namespace StockApp.ViewModel
{
    public class Orders
    {
        public List<BuyOrderResponse> BuyOrders { get; set; }

        public List<SellOrderResponse> SellOrders {  get; set; }
    }
}


using Microsoft.EntityFrameworkCore;
using StockApp.Core.Domain.Entities;
using StockApp.Core.Domain.RepositoryContracts;
using StockApp.Infrastructure.Data;

namespace StockApp.Infrastructure.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly StockMarketDbContext _context;

        public StocksRepository(StockMarketDbContext context)
        {
            _context = context;
        }

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _context.Add(buyOrder);
            await _context.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _context.Add(sellOrder);
            await _context.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            return await _context.buyOrders.ToListAsync();
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            return await _context.sellOrders.ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using StockApp.Core.Domain.Entities;


namespace StockApp.Infrastructure.Data
{
    public class StockMarketDbContext : DbContext
    {
        public StockMarketDbContext(DbContextOptions contextOptions) : base(contextOptions)
        {
            
        }

        public DbSet<BuyOrder> buyOrders { get; set; }
        public DbSet<SellOrder> sellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>(builder => { builder.ToTable("BuyOrders"); });
            modelBuilder.Entity<SellOrder>(builder => { builder.ToTable("SellOrders"); });
        }
    }
}

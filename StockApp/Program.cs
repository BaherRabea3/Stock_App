using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StockApp.Core.Domain.RepositoryContracts;
using StockApp.Core.ServiceContracts.FinhubServiceContract;
using StockApp.Core.ServiceContracts.StockServiceContract;
using StockApp.Core.Services.FinhubService;
using StockApp.Core.Services.StockService;
using StockApp.Infrastructure.Data;
using StockApp.Infrastructure.Repositories;
using StockApp.Middleware;

namespace StockApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Serilog
            builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => {

                loggerConfiguration
                .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
                .ReadFrom.Services(services); //read out current app's services and make them available to serilog
            });


            // Services
            builder.Services.AddControllersWithViews();
            builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
            builder.Services.AddScoped<IFinhubCompanyProfileService,FinhubCompanyProfileService>();
            builder.Services.AddScoped<IFinhubStockPriceQouteService, FinhubStockPriceQouteService>();
            builder.Services.AddScoped<IFinhubSearchStocksService, FinhubSearchStocksService>();
            builder.Services.AddScoped<IFinhubStocksService, FinhubStocksService>();
            builder.Services.AddScoped<IStockBuyOrderService, StockBuyOrderService>();
            builder.Services.AddScoped<IStockSellOrderService, StockSellOrderService>();
            builder.Services.AddScoped<IFinhubRepository, FinhubRepository>();
            builder.Services.AddScoped<IStocksRepository, StocksRepository>();


            builder.Services
              .AddDbContext<StockMarketDbContext>(options => options
              .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                           )
              );

            builder.Services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            builder.Services.AddHttpClient();
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseMiddleware<ExceptionHandlingMiddleware>();
            }


            if (builder.Environment.IsEnvironment("Test") == false)
                Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

            app.UseHttpLogging();

            app.UseStaticFiles();
           app.UseRouting();
           app.MapControllers();

            app.Run();
        }
    }
}

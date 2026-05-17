using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using StockApp.Core.DTOs;
using StockApp.Core.DTOs.BuyOrder;
using StockApp.Core.DTOs.SellOrder;
using StockApp.Core.ServiceContracts.FinhubServiceContract;
using StockApp.Core.ServiceContracts.StockServiceContract;
using StockApp.ViewModel;

namespace StockApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinhubStockPriceQouteService _finhubStockPriceQouteService;
        private readonly IFinhubCompanyProfileService _finhubCompanyProfileService;
        private readonly IStockBuyOrderService _stockBuyOrderService;
        private readonly IStockSellOrderService _stockSellOrderService;
        private readonly IOptions<TradingOptions> _tradeOptions;
        private readonly IConfiguration _configuration;

        public TradeController(IFinhubStockPriceQouteService finhubStockPriceQouteService,
            IFinhubCompanyProfileService finhubCompanyProfileService,
            IStockSellOrderService stockSellOrderService,
            IStockBuyOrderService stockBuyOrderService,
            IOptions<TradingOptions> stockOptions,
            IConfiguration configuration)
        {
            _finhubStockPriceQouteService = finhubStockPriceQouteService;
            _finhubCompanyProfileService = finhubCompanyProfileService;
            _stockSellOrderService = stockSellOrderService;
            _stockBuyOrderService = stockBuyOrderService;
            _tradeOptions = stockOptions;
            _configuration = configuration;
        }
        [Route("[action]/{stockSymbol?}")]
        //[Route("~/[controller]/{stockSymbol}")]
        public async Task<IActionResult> Index([FromRoute]string? stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";

            var FinhubStockPriceQouteResponse =
                await _finhubStockPriceQouteService.GetStockPriceQuote(stockSymbol);
            var FinhubProfileResponse =
                await _finhubCompanyProfileService.GetCompanyProfile(stockSymbol);

            StockTrade stockTrade = new StockTrade { StockSymbol = stockSymbol };

            if (FinhubProfileResponse != null && FinhubStockPriceQouteResponse != null)
            {
                stockTrade = new StockTrade()
                {
                    StockName = Convert.ToString(FinhubProfileResponse["name"]),
                    StockSymbol = Convert.ToString(FinhubProfileResponse["ticker"]),
                    Quantity = _tradeOptions.Value.DefaultOrderQuantity ?? 0,
                    Price = Convert.ToDouble(FinhubStockPriceQouteResponse["c"].ToString()),
                };
            }

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(buyOrderRequest);


            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = buyOrderRequest.StockName, Quantity = buyOrderRequest.Quantity, StockSymbol = buyOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            BuyOrderResponse buyOrderResponse = await _stockBuyOrderService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            SellOrderResponse sellOrderResponse = await _stockSellOrderService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction(nameof(Orders));
        }


        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrderResponses = await _stockBuyOrderService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stockSellOrderService.GetSellOrders();

            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            ViewBag.TradingOptions = _tradeOptions;

            return View(orders);
        }



        [Route("OrdersPDF")]
        public async Task<IActionResult> OrdersPDF()
        {
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await _stockBuyOrderService.GetBuyOrders());
            orders.AddRange(await _stockSellOrderService.GetSellOrders());
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            ViewBag.TradingOptions = _tradeOptions;

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}

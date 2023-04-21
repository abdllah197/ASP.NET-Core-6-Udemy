using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts.DTO;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using Stock_App.Filters.ActionFilters;
using Stock_App.Models;

namespace Stock_App.Controllers
{
    [Route("[Controller]")]
	public class TradeController : Controller
	{
		#region Injections

		private readonly TradingOptions _tradingOptions;
		private readonly IStocksBuyOrderService _stockBuyOrderService;
		private readonly IStocksSellOrderService _stockSellOrderService;
		private readonly IFinnhubGetService _finnhubGetService;
		private readonly IFinnhubSearchService _finnhubSearchService;
		private readonly IConfiguration _configuration;
		private readonly ILogger<TradeController> _logger;
		public TradeController(IFinnhubGetService finnhubGetService, IFinnhubSearchService finnhubSearchService, IStocksBuyOrderService stockCreateService, IStocksSellOrderService stockGetService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration,ILogger<TradeController> logger)
		{
			_tradingOptions = tradingOptions.Value;
			_finnhubGetService = finnhubGetService;
			_finnhubSearchService = finnhubSearchService;
			_stockBuyOrderService = stockCreateService;
			_stockSellOrderService = stockGetService;
			_configuration = configuration;
			_logger = logger;
		}
		#endregion

		#region Index
		[HttpGet]
		[Route("Index/{stockSymbol}")]
		public async Task<IActionResult> Index(string stockSymbol)
		{
            _logger.LogInformation("In {ClassName}.{MethodName}", nameof(TradeController), nameof(Index));
            if (stockSymbol == string.Empty)
				stockSymbol = "MSFT";
			Dictionary<string, object>? CompanyProfile = await _finnhubGetService.GetCompanyProfile(stockSymbol);
			Dictionary<string, object>? StockPriceQuote = await _finnhubGetService.GetStockPriceQuote(stockSymbol);

			StockTrade stockTrade = new StockTrade() { StockSymbol = stockSymbol };
			if (StockPriceQuote is not null && CompanyProfile is not null)
			{
				stockTrade = new StockTrade()
				{
					StockSymbol = Convert.ToString(CompanyProfile["ticker"]),
					StockName = Convert.ToString(CompanyProfile["name"]),
					Quantity = _tradingOptions.DefaultOrderQuantity ?? 0,
					Price = Convert.ToDouble(StockPriceQuote["c"].ToString())
				};
			}
			ViewBag.FinnhubToken = _configuration["FinnhubToken"];
			return View(stockTrade);
		}
		#endregion

		#region BuyOrder
		[HttpPost]
		[Route("[Action]")]
		[CreateOrderActionFactoryFilter]
		public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
		{			
			await _stockBuyOrderService.CreateBuyOrder(orderRequest);
			return RedirectToAction(nameof(Orders));
		}
		#endregion

		#region SellOrder
		[HttpPost]
		[Route("[Action]")]
        [CreateOrderActionFactoryFilter]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
		{			
			await _stockSellOrderService.CreateSellOrder(orderRequest);
			return RedirectToAction("Orders");
		}
		#endregion

		#region Orders
		[Route("[Action]")]
		public async Task<IActionResult> Orders()
		{
			List<BuyOrderResponse> buyOrders = await _stockBuyOrderService.GetBuyOrders();
			List<SellOrderResponse> sellOrders = await _stockSellOrderService.GetSellOrders();
			Orders orders = new Orders()
			{
				BuyOrders=buyOrders,
				SellOrders=sellOrders
			};
			return View(orders);
		}
		#endregion

		#region OrdersPDF
		[Route("[Action]")]
		public async Task<IActionResult> OrdersPDF()
		{
			List<IOrdersResponse> ordersResponse = new List<IOrdersResponse>();
			ordersResponse.AddRange(await _stockBuyOrderService.GetBuyOrders());
			ordersResponse.AddRange(await _stockSellOrderService.GetSellOrders());
			ViewBag.ordersResponse=ordersResponse.OrderByDescending(temp=>temp.DateAndTimeOfOrder).ToList();
			//pdf generation and html generation
			return new ViewAsPdf("OrdersPDF", ordersResponse, ViewData)
			{
				PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
				PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
			};
		}

		#endregion
	}
}

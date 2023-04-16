using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Stock_App.Filters.ActionFilters;
using Stock_App.Models;
using System.ComponentModel.DataAnnotations;

namespace Stock_App.Controllers
{
	[Route("[Controller]")]
	public class TradeController : Controller
	{
		#region Injections

		private readonly TradingOptions _tradingOptions;
		private readonly IStocksService _stockService;
		private readonly IFinnhubService _finnhubService;
		private readonly IConfiguration _configuration;
		private readonly ILogger<TradeController> _logger;
		public TradeController(IFinnhubService finnhubService, IStocksService stockService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration,ILogger<TradeController> logger)
		{
			_tradingOptions = tradingOptions.Value;
			_finnhubService = finnhubService;
			_stockService = stockService;
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
			Dictionary<string, object>? CompanyProfile = await _finnhubService.GetCompanyProfile(stockSymbol);
			Dictionary<string, object>? StockPriceQuote = await _finnhubService.GetStockPriceQuote(stockSymbol);

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
			await _stockService.CreateBuyOrder(orderRequest);
			return RedirectToAction(nameof(Orders));
		}
		#endregion

		#region SellOrder
		[HttpPost]
		[Route("[Action]")]
        [CreateOrderActionFactoryFilter]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
		{			
			await _stockService.CreateSellOrder(orderRequest);
			return RedirectToAction("Orders");
		}
		#endregion

		#region Orders
		[Route("[Action]")]
		public async Task<IActionResult> Orders()
		{
			List<BuyOrderResponse> buyOrders = await _stockService.GetBuyOrders();
			List<SellOrderResponse> sellOrders = await _stockService.GetSellOrders();
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
			ordersResponse.AddRange(await _stockService.GetBuyOrders());
			ordersResponse.AddRange(await _stockService.GetSellOrders());
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

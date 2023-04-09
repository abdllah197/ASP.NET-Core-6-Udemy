using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
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
		public TradeController(IFinnhubService finnhubService, IStocksService stockService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
		{
			_tradingOptions = tradingOptions.Value;
			_finnhubService = finnhubService;
			_stockService = stockService;
			_configuration = configuration;
		}
		#endregion

		#region Index
		[HttpGet]
		[Route("Index/{stockSymbol}")]
		public async Task<IActionResult> Index(string stockSymbol)
		{
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
		public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
		{
			buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
			ModelState.Clear();
			TryValidateModel(buyOrderRequest);
			if (!ModelState.IsValid)
			{
				ViewBag.Errors = ModelState.Values.SelectMany(e => e.Errors).Select(x => x.ErrorMessage).ToList();
				StockTrade stockTrade = new StockTrade() { StockName = buyOrderRequest.StockName, Price = buyOrderRequest.Price, Quantity = buyOrderRequest.Quantity, StockSymbol = buyOrderRequest.StockSymbol };
				return View("Index", stockTrade);
			}
			await _stockService.CreateBuyOrder(buyOrderRequest);
			return RedirectToAction(nameof(Orders));
		}
		#endregion

		#region SellOrder
		[HttpPost]
		[Route("[Action]")]
		public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
		{
			sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
			ModelState.Clear();
			TryValidateModel(sellOrderRequest);
			if (!ModelState.IsValid)
			{
				ViewBag.Errors = ModelState.Values.SelectMany(e => e.Errors).Select(x => x.ErrorMessage).ToList();
				StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Price = sellOrderRequest.Price, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
				return View("Index", stockTrade);
			}
			await _stockService.CreateSellOrder(sellOrderRequest);
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

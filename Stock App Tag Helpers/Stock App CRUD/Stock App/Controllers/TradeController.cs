using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Stock_App.Models;
using System.ComponentModel.DataAnnotations;

namespace Stock_App.Controllers
{
    
    public class TradeController : Controller
	{
		private readonly TradingOptions _tradingOptions;
		private readonly IStockService _stockService;
		private readonly IFinnhubService _finnhubService;
		private readonly IConfiguration _configuration;
		public TradeController(IFinnhubService finnhubService,IStockService stockService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
		{
			_tradingOptions = tradingOptions.Value;
			_finnhubService = finnhubService;
			_stockService = stockService;
			_configuration = configuration;
		}
        [HttpGet]
        [Route("/")]		
        [Route("Index")]		
        public IActionResult Index()
		{
			
            Dictionary<string, object>? StockPriceQuote = _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);
			Dictionary<string, object>? CompanyProfile = _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

			StockTrade stockTrade = new StockTrade() { StockSymbol = _tradingOptions.DefaultStockSymbol };
			if (StockPriceQuote != null && CompanyProfile != null)
			{
				stockTrade = new StockTrade()
				{
					StockSymbol = Convert.ToString(CompanyProfile["ticker"]),
					StockName = Convert.ToString(CompanyProfile["name"]),
					Price = Convert.ToDouble(StockPriceQuote["c"].ToString())
				};
			}
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];
            return View(stockTrade);
		}
		
		[HttpPost]
        [Route("BuyOrder")]
        public IActionResult BuyOrder(BuyOrderRequest buyOrderRequest)
		{
			buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
			ModelState.Clear();
            TryValidateModel(buyOrderRequest);
            if (!ModelState.IsValid) 
			{
				ViewBag.Errors = ModelState.Values.SelectMany(e => e.Errors).Select(x => x.ErrorMessage).ToList();
				StockTrade stockTrade= new StockTrade() { StockName=buyOrderRequest.StockName, Price=buyOrderRequest.Price,Quantity=buyOrderRequest.Quantity,StockSymbol=buyOrderRequest.StockSymbol};
				return View("Index",stockTrade);
			}
            BuyOrderResponse buyOrderResponse = _stockService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        [Route("SellOrder")]
        public IActionResult SellOrder(SellOrderRequest sellOrderRequest)
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
            SellOrderResponse sellOrderResponse = _stockService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction("Orders");
        }

		[Route("Orders")]
		public IActionResult Orders()
		{
			ViewBag.BuyOrders=_stockService.GetBuyOrders();
			ViewBag.SellOrders=_stockService.GetSellOrders();
			return View();
		}
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services;
using Stock_App.Models;

namespace Stock_App.Controllers
{
    
    public class TradeController : Controller
	{
		private readonly TradingOptions _tradingOptions;
		private readonly IFinnhubService _finnhubService;
		private readonly IConfiguration _configuration;
		public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
		{
			_tradingOptions = tradingOptions.Value;
			_finnhubService = finnhubService;
			_configuration = configuration;
		}
        [Route("/")]
        
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
	}
}

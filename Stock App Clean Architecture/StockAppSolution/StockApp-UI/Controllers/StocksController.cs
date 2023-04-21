using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.FinnhubService;
using Stock_App.Models;

namespace Stock_App.Controllers
{
    [Route("[Controller]")]
	public class StocksController : Controller
	{
		#region Injection
		private readonly TradingOptions tradingOptions;
		private readonly IFinnhubGetService finnhubService;
		public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubGetService finnhubService)
		{
			this.tradingOptions = tradingOptions.Value;
			this.finnhubService = finnhubService;
		}
		#endregion

		#region Explore
		[Route("/")]
		[Route("[Action]/{stock?}")]
		[Route("~/[Action]/{stock?}")]
		public async Task<IActionResult> Explore(string? stock, bool showAll = false)
		{
			List<Dictionary<string, string>>? stocksDictionary = await finnhubService.GetStocks();
			List<Stock> stocks = new List<Stock>();
			if (stocksDictionary is not null)
			{
				if (showAll is false && tradingOptions.Top25PopularStocks is not null)
				{
					string[]? Top25PopularStocksList = tradingOptions.Top25PopularStocks.Split(",");
					if (Top25PopularStocksList is not null)
					{
						stocksDictionary = stocksDictionary
							.Where(temp => Top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
							.ToList();
					}
				}
				stocks = stocksDictionary
					.Select(temp => new Stock()
					{
						StockName = Convert.ToString(temp["description"]),
						StockSymbol = Convert.ToString(temp["symbol"])
					})
					.ToList();
			}
			ViewBag.stock = stock;
			return View(stocks);
		}
		#endregion
	}
}

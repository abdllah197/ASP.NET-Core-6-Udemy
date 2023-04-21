using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;

namespace Stock_App.ViewComonents
{
    public class SelectedStockViewComponent:ViewComponent
    {
        private readonly IFinnhubGetService _finnhubService;
        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IStocksBuyOrderService stocksService, IFinnhubGetService finnhubService, IConfiguration configuration)
        {            
            _finnhubService = finnhubService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDict = null;

            if (stockSymbol != null)
            {
                companyProfileDict = await _finnhubService.GetCompanyProfile(stockSymbol);
                var stockPriceDict = await _finnhubService.GetStockPriceQuote(stockSymbol);
                if (stockPriceDict != null && companyProfileDict != null)
                {
                    companyProfileDict.Add("price", stockPriceDict["c"]);
                }
            }

            if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
                return View(companyProfileDict);
            else
                return Content("");
        }

    }
}

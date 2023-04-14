
using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using System.Text.Json;

namespace Services
{
	public class FinnhubService : IFinnhubService
	{
		#region Injections
		private readonly IFinnhubRepository _finnhubrepository;
		public FinnhubService(IFinnhubRepository finnhubrepository) 
		{ 
			this._finnhubrepository = finnhubrepository;
		}
		#endregion

		#region GetCompanyProfile
		public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
		{
			Dictionary<string, object>? CompanyProfile=await _finnhubrepository.GetCompanyProfile(stockSymbol);
			return CompanyProfile;
		}
        #endregion

        #region GetStockPriceQuote
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
		{
			Dictionary<string, object>? StockPriceQuote = await _finnhubrepository.GetStockPriceQuote(stockSymbol);
			return StockPriceQuote;
		}
		#endregion

		#region GetStocks
		public async Task<List<Dictionary<string, string>>?> GetStocks()
		{
			List<Dictionary<string, string>>? Stocks = await _finnhubrepository.GetStocks();
			return Stocks;
		}
		#endregion

		#region SearchStocks
		public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
		{
			Dictionary<string, object>? stockSearchResult = await _finnhubrepository.GetCompanyProfile(stockSymbolToSearch);
			return stockSearchResult;
		}
		#endregion
	}
}
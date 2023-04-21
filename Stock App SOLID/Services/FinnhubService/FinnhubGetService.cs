
using Exceptions;
using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using ServiceContracts.FinnhubService;
using System.Text.Json;

namespace Services.FinnhubService
{
    public class FinnhubGetService : IFinnhubGetService
    {
        #region Injections
        private readonly IFinnhubRepository _finnhubrepository;
        public FinnhubGetService(IFinnhubRepository finnhubrepository)
        {
            _finnhubrepository = finnhubrepository;
        }
        #endregion

        #region GetCompanyProfile
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            try
            {
                Dictionary<string, object>? CompanyProfile = await _finnhubrepository.GetCompanyProfile(stockSymbol);
                return CompanyProfile;
            }
            catch (Exception ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }
        #endregion

        #region GetStockPriceQuote
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            try
            {
                Dictionary<string, object>? StockPriceQuote = await _finnhubrepository.GetStockPriceQuote(stockSymbol);
                return StockPriceQuote;
            }
            catch (Exception ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }
        #endregion

        #region GetStocks
        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            try
            {
                List<Dictionary<string, string>>? Stocks = await _finnhubrepository.GetStocks();
                return Stocks;
            }
            catch (Exception ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }
        #endregion

        #region SearchStocks
        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            try
            {
                Dictionary<string, object>? stockSearchResult = await _finnhubrepository.GetCompanyProfile(stockSymbolToSearch);
                return stockSearchResult;
            }
            catch (Exception ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }
        #endregion
    }
}
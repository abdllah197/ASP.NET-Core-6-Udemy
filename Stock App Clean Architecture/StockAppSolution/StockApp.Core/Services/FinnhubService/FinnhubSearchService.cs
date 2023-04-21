
using Exceptions;
using RepositoryContracts;
using ServiceContracts.FinnhubService;

namespace Services.FinnhubService
{
    public class FinnhubSearchService : IFinnhubSearchService
    {
        #region Injections
        private readonly IFinnhubRepository _finnhubrepository;
        public FinnhubSearchService(IFinnhubRepository finnhubSearchrepository)
        {
            _finnhubrepository = finnhubSearchrepository;
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
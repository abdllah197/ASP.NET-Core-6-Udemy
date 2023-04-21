using ServiceContracts.DTO;

namespace ServiceContracts.FinnhubService
{
    public interface IFinnhubGetService
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

        Task<List<Dictionary<string, string>>?> GetStocks();
    }
}
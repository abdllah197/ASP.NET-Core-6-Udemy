using ServiceContracts.DTO;

namespace ServiceContracts.FinnhubService
{
    public interface IFinnhubSearchService
    {
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}
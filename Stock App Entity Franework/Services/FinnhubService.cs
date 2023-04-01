
using Microsoft.Extensions.Configuration;
using ServiceContracts.DTO;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
	public class FinnhubService : IFinnhubService
	{
		#region ServiceInjection

		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;
		public FinnhubService(IHttpClientFactory httpClientFactory,IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}
		#endregion
		
        #region GetCompanyProfil
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
		{
			HttpClient httpClient = _httpClientFactory.CreateClient();
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
			};
			HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
			string responseMessage = await httpResponseMessage.Content.ReadAsStringAsync();
			Dictionary<string, object>? result = JsonSerializer.Deserialize<Dictionary<string, object>>(responseMessage);

			if (result == null)
			{
				throw new InvalidOperationException("No response from server");
			}
			if (result.ContainsKey("error"))
			{
				throw new InvalidOperationException(Convert.ToString(result["error"]));
			}
			return result;
		}
        #endregion

        #region GetStockPriceQuote
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
		{
			HttpClient httpClient = _httpClientFactory.CreateClient();
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
			};
			HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
			string responseMessage = await httpResponseMessage.Content.ReadAsStringAsync();
			Dictionary<string, object>? result = JsonSerializer.Deserialize<Dictionary<string, object>>(responseMessage);

			if (result == null)
			{
				throw new InvalidOperationException("No response from server");
			}
			if (result.ContainsKey("error"))
			{
				throw new InvalidOperationException(Convert.ToString(result["error"]));
			}
			return result;
		}
        #endregion
    }
}
﻿using ServiceContracts.DTO;

namespace Services
{
	public interface IFinnhubService
	{
		Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

		Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
        
    }
}
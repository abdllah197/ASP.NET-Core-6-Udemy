﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
	public interface IFinnhubRepository
	{
		Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

		Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

		Task<List<Dictionary<string, string>>?> GetStocks();

		Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
	public interface IOrdersResponse
	{
		string StockSymbol { get; set; }
		string StockName { get; set; }

		DateTime DateAndTimeOfOrder { get; set; }

		uint Quantity { get; set; }

		double Price { get; set; }

		OrderType TypeOfOrder { get; }

		double TradeAmount { get; set; }
	}

	public enum OrderType
	{
		BuyOrder, SellOrder
	}
}


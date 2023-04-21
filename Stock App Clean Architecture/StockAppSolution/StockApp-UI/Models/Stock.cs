namespace Stock_App.Models
{
	public class Stock
	{
		public string? StockSymbol { get; set; }
		public string? StockName { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj == null) return false;
			if (obj is not Stock) return false;
			Stock that = (Stock)obj;
			return StockName==that.StockName && StockSymbol==that.StockSymbol;
		}

		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}
	}
}

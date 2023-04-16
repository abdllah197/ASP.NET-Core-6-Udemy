using ServiceContracts.DTO;

namespace Stock_App.Models
{
	public class Orders
	{
		public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();

		public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();
	}
}

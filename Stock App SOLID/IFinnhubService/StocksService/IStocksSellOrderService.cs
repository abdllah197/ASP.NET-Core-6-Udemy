using ServiceContracts.DTO;

namespace ServiceContracts.StocksService
{
    public interface IStocksSellOrderService
    {
		Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

		Task<List<SellOrderResponse>> GetSellOrders();
    }
}

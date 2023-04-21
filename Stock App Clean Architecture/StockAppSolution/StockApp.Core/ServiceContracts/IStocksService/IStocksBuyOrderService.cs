using ServiceContracts.DTO;

namespace ServiceContracts.StocksService
{
    public interface IStocksBuyOrderService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

		Task<List<BuyOrderResponse>> GetBuyOrders();
    }
}

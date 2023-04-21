using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.StocksService;
using StockApp.Core.Helpers;

namespace Services.StocksService
{
    public class StocksBuyOrderService : IStocksBuyOrderService
    {
        #region Injections
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksBuyOrderService> _logger;
        public StocksBuyOrderService(IStocksRepository stocksRepository, ILogger<StocksBuyOrderService> logger)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
        }
        #endregion

        #region CreateBuyOrder
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            _logger.LogInformation("In {ClassName}.{MethodName}", nameof(StocksBuyOrderService), nameof(CreateBuyOrder));
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            await _stocksRepository.CreateBuyOrder(buyOrder);
            return buyOrder.ToBuyOrderResponse();

        }
		#endregion

		#region GetBuyOrders
		public async Task<List<BuyOrderResponse>> GetBuyOrders()
		{
			_logger.LogInformation("In {ClassName}.{MethodName}", nameof(StocksSellOrderService), nameof(GetBuyOrders));
			List<BuyOrder> DbContextbuyOrders = await _stocksRepository.GetBuyOrders();
			List<BuyOrderResponse> BuyOrderResponseList = new List<BuyOrderResponse>();
			foreach (BuyOrder buyOrder in DbContextbuyOrders)
			{
				BuyOrderResponseList.Add(buyOrder.ToBuyOrderResponse());
			}
			return BuyOrderResponseList;
		}
		#endregion


	}
}

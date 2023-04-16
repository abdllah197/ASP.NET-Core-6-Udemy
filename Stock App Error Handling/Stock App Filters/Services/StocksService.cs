using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StocksService:IStocksService
    {
		#region Injections
		private readonly IStocksRepository stocksRepository;
        private readonly ILogger<StocksService> _logger;
        public StocksService(IStocksRepository stocksRepository,ILogger<StocksService> logger)
        {
            this.stocksRepository = stocksRepository;
            _logger = logger;
        }
		#endregion

		#region CreateBuyOrder
		public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            _logger.LogInformation("In {ClassName}.{MethodName}", nameof(StocksService), nameof(CreateBuyOrder));
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID=Guid.NewGuid();
            await stocksRepository.CreateBuyOrder(buyOrder);
			return buyOrder.ToBuyOrderResponse();
            
        }
		#endregion

		#region CreateSellOrder
		public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            _logger.LogInformation("In {ClassName}.{MethodName}", nameof(StocksService), nameof(CreateSellOrder));
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            await stocksRepository.CreateSellOrder(sellOrder);
			return sellOrder.ToSellOrderResponse();
        }
		#endregion

		#region GetBuyOrders
		public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            _logger.LogInformation("In {ClassName}.{MethodName}", nameof(StocksService), nameof(GetBuyOrders));
            List<BuyOrder> DbContextbuyOrders = await stocksRepository.GetBuyOrders();
            List<BuyOrderResponse> BuyOrderResponseList = new List<BuyOrderResponse>();
            foreach(BuyOrder buyOrder in DbContextbuyOrders)
            {
                BuyOrderResponseList.Add(buyOrder.ToBuyOrderResponse());
            }
            return BuyOrderResponseList;
        }
		#endregion

		#region GetSellOrders
		public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            _logger.LogInformation("In {ClassName}.{MethodName}", nameof(StocksService), nameof(GetSellOrders));
            List<SellOrder> DbContextSellOrders =await stocksRepository.GetSellOrders();
            List<SellOrderResponse> SellOrderResponseList = new List<SellOrderResponse>();
            foreach (SellOrder sellOrder in DbContextSellOrders)
            {
                SellOrderResponseList.Add(sellOrder.ToSellOrderResponse());
            }
            return SellOrderResponseList;
        }
		#endregion
	}
}

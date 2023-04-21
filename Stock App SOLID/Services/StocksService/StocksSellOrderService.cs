using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.StocksService;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StocksService
{
    public class StocksSellOrderService : IStocksSellOrderService
    {
        #region Injections
        private readonly IStocksRepository _stocksRepository;
        private readonly ILogger<StocksSellOrderService> _logger;
        public StocksSellOrderService(IStocksRepository stocksRepository, ILogger<StocksSellOrderService> logger)
        {
            _stocksRepository = stocksRepository;
            _logger = logger;
        }
		#endregion

		#region CreateSellOrder
		public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
		{
			_logger.LogInformation("In {ClassName}.{MethodName}", nameof(StocksBuyOrderService), nameof(CreateSellOrder));
			if (sellOrderRequest == null)
				throw new ArgumentNullException(nameof(sellOrderRequest));

			ValidationHelper.ModelValidation(sellOrderRequest);

			SellOrder sellOrder = sellOrderRequest.ToSellOrder();
			sellOrder.SellOrderID = Guid.NewGuid();
			await _stocksRepository.CreateSellOrder(sellOrder);
			return sellOrder.ToSellOrderResponse();
		}
		#endregion

        #region GetSellOrders
        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            _logger.LogInformation("In {ClassName}.{MethodName}", nameof(StocksSellOrderService), nameof(GetSellOrders));
            List<SellOrder> DbContextSellOrders = await _stocksRepository.GetSellOrders();
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

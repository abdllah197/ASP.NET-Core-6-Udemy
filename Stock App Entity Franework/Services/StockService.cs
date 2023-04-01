using Entities;
using Microsoft.EntityFrameworkCore;
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
    public class StockService:IStockService
    {
        private readonly StockMarketDbContext _DbContext;
        public StockService(StockMarketDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID=Guid.NewGuid();
            _DbContext.BuyOrders.Add(buyOrder);
			await _DbContext.SaveChangesAsync();
			return buyOrder.ToBuyOrderResponse();
            
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            _DbContext.SellOrders.Add(sellOrder);
			await _DbContext.SaveChangesAsync();
			return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            List<BuyOrder> DbContextbuyOrders= await _DbContext.BuyOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToListAsync();
            List<BuyOrderResponse> BuyOrderResponseList = new List<BuyOrderResponse>();
            foreach(BuyOrder buyOrder in DbContextbuyOrders)
            {
                BuyOrderResponseList.Add(buyOrder.ToBuyOrderResponse());
            }
            return BuyOrderResponseList;
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> DbContextSellOrders =await _DbContext.SellOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToListAsync();
            List<SellOrderResponse> SellOrderResponseList = new List<SellOrderResponse>();
            foreach (SellOrder sellOrder in DbContextSellOrders)
            {
                SellOrderResponseList.Add(sellOrder.ToSellOrderResponse());
            }
            return SellOrderResponseList;
        }
    }
}

using Entities;
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
        private readonly List<BuyOrder>_buyOrders;
        private readonly List<SellOrder>_sellOrders;
        public StockService()
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }
        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            ValidationHelper.ModelValidation(buyOrderRequest);

            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID=Guid.NewGuid();
            _buyOrders.Add(buyOrder);
            return buyOrder.ToBuyOrderResponse();
            
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            _sellOrders.Add(sellOrder);
            return sellOrder.ToSellOrderResponse();
        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            List<BuyOrderResponse> BuyOrderResponseList = new List<BuyOrderResponse>();
            foreach(BuyOrder buyOrder in _buyOrders)
            {
                BuyOrderResponseList.Add(buyOrder.ToBuyOrderResponse());
            }
            return BuyOrderResponseList;
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            List<SellOrderResponse> SellOrderResponseList = new List<SellOrderResponse>();
            foreach (SellOrder sellOrder in _sellOrders)
            {
                SellOrderResponseList.Add(sellOrder.ToSellOrderResponse());
            }
            return SellOrderResponseList;
        }
    }
}

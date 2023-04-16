using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Security.Cryptography;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Tests
{
    public class StocksServiceTest
    {
        #region ServicesInjection
        private readonly IStockService _stockService;
        private readonly ITestOutputHelper _testOutputHelper;
        public StocksServiceTest(ITestOutputHelper testOutputHelper) 
        {
            _stockService=new StockService();
            _testOutputHelper=testOutputHelper;
        }
        #endregion

        #region DummyDataCreation
        public BuyOrderRequest? BuyOrderdumyData()
        {
            int RandomNumber=new Random().Next(1000,9999);
            return new BuyOrderRequest()
            {
                StockName = "Dummy Name#"+RandomNumber,
                StockSymbol = "Dummy Symbol#" + RandomNumber,
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = Convert.ToUInt16(new Random().Next(1, 100)),
                Price = new Random().Next(50, 500)
            };
        }
        
        public SellOrderRequest? SellOrderdumyData()
        {
            int RandomNumber = new Random().Next(1000, 9999);
            return new SellOrderRequest()
            {
                StockName = "Dummy Name#" + RandomNumber,
                StockSymbol = "Dummy Symbol#" + RandomNumber,
                DateAndTimeOfOrder = DateTime.Now,
                Quantity = Convert.ToUInt16(new Random().Next(1, 100)),
                Price = new Random().Next(50, 500)
            };
        }

        #endregion

        #region CreateBuyOrder

        [Fact]
        public void CreateBuyOrder_BuyOrderIsNull()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_QuantityIsLessThanMinimum()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = BuyOrderdumyData();
            if (buyOrderRequest != null)
            {
                buyOrderRequest.Quantity = 0;
            }
            
            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }
        
        [Fact]
        public void CreateBuyOrder_QuantityIsBiggerThanMaximum()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = BuyOrderdumyData();
            if (buyOrderRequest != null)
            {
                buyOrderRequest.Quantity = 100001;
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_PriceIsLessThanMinimum()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = BuyOrderdumyData();
            if (buyOrderRequest != null)
            {
                buyOrderRequest.Price = 0;
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_PriceIsBiggerThanMaximum()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = BuyOrderdumyData();
            if (buyOrderRequest != null)
            {
                buyOrderRequest.Price = 10001;
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_StockSymbolIsNull()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = BuyOrderdumyData();
            if (buyOrderRequest != null)
            {
                buyOrderRequest.StockSymbol = null;
            }
            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_DateAndTimeOfOrderIsLessThanMinimum()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = BuyOrderdumyData();
            if (buyOrderRequest != null)
            {
                buyOrderRequest.DateAndTimeOfOrder =  Convert.ToDateTime("1999-12-31");
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateBuyOrder(buyOrderRequest);
            });
        }

        [Fact]
        public void CreateBuyOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = BuyOrderdumyData();

            //Act
            BuyOrderResponse? buyOrderResponse=_stockService.CreateBuyOrder(buyOrderRequest);

            //Assert
            Assert.NotEqual(Guid.Empty, buyOrderResponse.BuyOrderID);
        }

        #endregion

        #region CreateSellOrder
        [Fact]
        public void CreateSellOrder_SellOrderIsNull()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_QuantityIsLessThanMinimum()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = SellOrderdumyData();
            if (sellOrderRequest != null)
            {
                sellOrderRequest.Quantity = 0;
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_QuantityIsBiggerThanMaximum()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = SellOrderdumyData();
            if (sellOrderRequest != null)
            {
                sellOrderRequest.Quantity = 100001;
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_PriceIsLessThanMinimum()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = SellOrderdumyData();
            if (sellOrderRequest != null)
            {
                sellOrderRequest.Price = 0;
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_PriceIsBiggerThanMaximum()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = SellOrderdumyData();
            if (sellOrderRequest != null)
            {
                sellOrderRequest.Price = 10001;
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_StockSymbolIsNull()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = SellOrderdumyData();
            if (sellOrderRequest != null)
            {
                sellOrderRequest.StockSymbol = null;
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_DateAndTimeOfOrderIsLessThanMinimum()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = SellOrderdumyData();
            if (sellOrderRequest != null)
            {
                sellOrderRequest.DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31");
            }

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _stockService.CreateSellOrder(sellOrderRequest);
            });
        }

        [Fact]
        public void CreateSellOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = SellOrderdumyData();

            //Act
            SellOrderResponse? sellOrderResponse = _stockService.CreateSellOrder(sellOrderRequest);

            //Assert
            Assert.NotEqual(Guid.Empty, sellOrderResponse.SellOrderID);
        }
        #endregion

        #region GetAllBuyOrders
        [Fact]
        public void GetAllBuyOrders_ByDefaultIsEmpty()
        {
            //Act
            List<BuyOrderResponse> buyOrderResponses=_stockService.GetBuyOrders();

            //Assert
            Assert.Empty(buyOrderResponses);

        }

        [Fact]
        public void GetAllBuyOrders_WithFewBuyOrderIsSuccessful()
        {
            //Arrange
            List<BuyOrderRequest> buyOrderRequests = new List<BuyOrderRequest>();
            buyOrderRequests.Add(BuyOrderdumyData()!);
            buyOrderRequests.Add(BuyOrderdumyData()!);
            buyOrderRequests.Add(BuyOrderdumyData()!);

            List<BuyOrderResponse> buyOrderResponsesList_FromAdd = new List<BuyOrderResponse>();
            foreach(BuyOrderRequest buyOrder_Request in buyOrderRequests)
            {
                BuyOrderResponse buyOrder_Response=_stockService.CreateBuyOrder(buyOrder_Request);
                buyOrderResponsesList_FromAdd.Add(buyOrder_Response);
            }
            //Act
            List<BuyOrderResponse> buyOrderResponsesList_FromGet=_stockService.GetBuyOrders();

            //Assert
            foreach (BuyOrderResponse buyOrderResponse_FromAdd in buyOrderResponsesList_FromAdd)
            {
                Assert.Contains(buyOrderResponse_FromAdd,buyOrderResponsesList_FromGet);
            }


        }

        #endregion

        #region GettAllSellOrders

        [Fact]
        public void GetAllSellOrders_ByDefaultIsEmpty()
        {
            //Act
            List<SellOrderResponse> sellOrderResponses = _stockService.GetSellOrders();

            //Assert
            Assert.Empty(sellOrderResponses);

        }

        [Fact]
        public void GetAllSellOrders_WithFewSellOrderIsSuccessful()
        {
            //Arrange
            List<SellOrderRequest> sellOrderRequests = new List<SellOrderRequest>();
            sellOrderRequests.Add(SellOrderdumyData()!);
            sellOrderRequests.Add(SellOrderdumyData()!);
            sellOrderRequests.Add(SellOrderdumyData()!);

            List<SellOrderResponse> sellOrderResponsesList_FromAdd = new List<SellOrderResponse>();
            foreach (SellOrderRequest sellOrder_Request in sellOrderRequests)
            {
                SellOrderResponse sellOrder_Response = _stockService.CreateSellOrder(sellOrder_Request);
                sellOrderResponsesList_FromAdd.Add(sellOrder_Response);
            }
            //Act
            List<SellOrderResponse> sellOrderResponsesList_FromGet = _stockService.GetSellOrders();

            //Assert
            foreach (SellOrderResponse sellOrderResponse_FromAdd in sellOrderResponsesList_FromAdd)
            {
                Assert.Contains(sellOrderResponse_FromAdd, sellOrderResponsesList_FromGet);
            }


        }

        #endregion
    }
}
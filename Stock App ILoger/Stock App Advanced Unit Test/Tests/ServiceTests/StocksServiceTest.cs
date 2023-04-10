using AutoFixture;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;
using RepositoryContracts;
using Entities;
using FluentAssertions;
using System;

namespace Tests.ServiceTests
{
    public class StocksServiceTest
    {
        #region Injections
        private readonly IStocksService _stocksService;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IStocksRepository _stocksRepository;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        public StocksServiceTest(ITestOutputHelper testOutputHelper)
        {
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;
            _stocksService = new StocksService(_stocksRepository);
            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();
        }
        #endregion

        #region CreateBuyOrder

        [Fact]
        public async Task CreateBuyOrder_BuyOrderIsNull_ToBeArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Mock
            BuyOrder buyOrderFixutre = _fixture.Build<BuyOrder>().Create();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrderFixutre);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, buyOrderQuantity)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(100001)]
        public async Task CreateBuyOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, buyOrderQuantity)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_PriceIsLessThanMinimum_ToBeArgumentException(double BuyOrderPrice)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, BuyOrderPrice)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(10001)]
        public async Task CreateBuyOrder_PriceIsBiggerThanMaximum_ToBeArgumentException(double BuyOrderPrice)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, BuyOrderPrice)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_DateAndTimeOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateBuyOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            BuyOrderResponse buyOrderResponseFromCreate = await _stocksService.CreateBuyOrder(buyOrderRequest);


            //Assert
            buyOrder.BuyOrderID = buyOrderResponseFromCreate.BuyOrderID;
            BuyOrderResponse buyOrderResponse_expected = buyOrder.ToBuyOrderResponse();
            buyOrderResponseFromCreate.Should().NotBe(Guid.Empty);
            buyOrderResponseFromCreate.Should().Be(buyOrderResponse_expected);

        }

        #endregion

        #region CreateSellOrder
        [Fact]
        public async Task CreateSellOrder_SellOrderIsNull_ToBeArgumentNullException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Mock
            SellOrder sellOrderFixutre = _fixture.Build<SellOrder>().Create();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrderFixutre);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, buyOrderQuantity)
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(100001)]
        public async Task CreateSellOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint SellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, SellOrderQuantity)
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_PriceIsLessThanMinimum_ToBeArgumentException(double SellOrderPrice)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, SellOrderPrice)
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData(10001)]
        public async Task CreateSellOrder_PriceIsBiggerThanMaximum_ToBeArgumentException(double SellOrderPrice)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, SellOrderPrice)
                .Create();

            //Mock
            SellOrder buyOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_DateAndTimeOfOrderIsLessThanYear2000_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateSellOrder_ValidData_ToBeSuccessful()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            SellOrderResponse sellOrderResponseFromCreate = await _stocksService.CreateSellOrder(sellOrderRequest);


            //Assert
            sellOrder.SellOrderID = sellOrderResponseFromCreate.SellOrderID;
            SellOrderResponse sellOrderResponse_expected = sellOrder.ToSellOrderResponse();
            sellOrderResponseFromCreate.Should().NotBe(Guid.Empty);
            sellOrderResponseFromCreate.Should().Be(sellOrderResponse_expected);

        }

        #endregion

        #region GetAllBuyOrders
        [Fact]
        public async Task GetAllBuyOrders_ByDefaultIsEmpty_ToBeArgumentException()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>();


            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            //Act


            List<BuyOrderResponse> buyOrdersFromGet = await _stocksService.GetBuyOrders();


            //Assert
            Assert.Empty(buyOrdersFromGet);

        }

        [Fact]
        public async Task GetAllBuyOrders_WithFewBuyOrder_ToBeSuccessful()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>(){
                _fixture.Build<BuyOrder>()
                .Create(),
                _fixture.Build<BuyOrder>()
                .Create(),
            };
            List<BuyOrderResponse> BuyOrdersResponseFromAdd = buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            //Act


            List<BuyOrderResponse> buyOrdersRespnseFromGet = await _stocksService.GetBuyOrders();


            //Assert
            BuyOrdersResponseFromAdd.Should().BeEquivalentTo(buyOrdersRespnseFromGet);
        }

        #endregion

        #region GettAllSellOrders

        [Fact]
        public async Task GetAllSellOrders_ByDefaultIsEmpty_ToBeArgumentException()
        {
            //Arrange
            List<SellOrder> buyOrders = new List<SellOrder>();


            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(buyOrders);

            //Act


            List<SellOrderResponse> sellOrdersFromGet = await _stocksService.GetSellOrders();


            //Assert
            Assert.Empty(sellOrdersFromGet);

        }

        [Fact]
        public async Task GetAllSellOrders_WithFewBuyOrder_ToBeSuccessful()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>(){
                _fixture.Build<SellOrder>()
                .Create(),
                _fixture.Build<SellOrder>()
                .Create(),
            };
            List<SellOrderResponse> SellOrdersResponseFromAdd = sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            //Act


            List<SellOrderResponse> sellOrdersRespnseFromGet = await _stocksService.GetSellOrders();


            //Assert
            SellOrdersResponseFromAdd.Should().BeEquivalentTo(sellOrdersRespnseFromGet);
        }


        #endregion
    }
}
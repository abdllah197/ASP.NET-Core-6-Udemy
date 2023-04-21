
using AutoFixture;
using Moq;
using FluentAssertions;
using Stock_App;
using Stock_App.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Stock_App.Models;
using Serilog;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using Microsoft.Extensions.Logging;

namespace Tests.ControllerTests
{
    public class TradeControllerTest
    {
        private readonly IFinnhubGetService _finnhubGetService;
        private readonly IFinnhubSearchService _finnhubSearchService;
        private readonly IStocksBuyOrderService _stocksBuyOrderService;
        private readonly IStocksSellOrderService _stocksSellOrderService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TradeController> _logger;
        private readonly Mock <IFinnhubGetService> _finnhubGetServiceMock;
        private readonly Mock <IFinnhubSearchService> _finnhubSearchServiceMock;
        private readonly Mock <IStocksBuyOrderService> _stocksBuyOrderServiceMock;
        private readonly Mock <IStocksSellOrderService> _stocksSellOrderServiceMock;
        private readonly Mock <ILogger<TradeController>> _loggerMock;
        private readonly Mock <IConfiguration> _configurationMock;
        private readonly Fixture _fixture;
        

       public TradeControllerTest()
        {
            _finnhubGetServiceMock = new Mock<IFinnhubGetService>();
            _finnhubSearchServiceMock = new Mock<IFinnhubSearchService>();
            _stocksSellOrderServiceMock = new Mock<IStocksSellOrderService>();
            _stocksBuyOrderServiceMock = new Mock<IStocksBuyOrderService>();
            _loggerMock = new Mock<ILogger<TradeController>>();
            _configurationMock = new Mock<IConfiguration>();
            _finnhubGetService = _finnhubGetServiceMock.Object;
            _finnhubSearchService = _finnhubSearchServiceMock.Object;
            _stocksBuyOrderService = _stocksBuyOrderServiceMock.Object;
            _stocksSellOrderService = _stocksSellOrderServiceMock.Object;
            _logger=_loggerMock.Object;
            _configuration= _configurationMock.Object;
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Index_StockSymbolIsEmpty_ShouldReturnIndexViewWithStockTrade()
        {
            //Arrange
            Dictionary<string, object>? CompanyProfile = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(@"{'name':'TestName','ticker':'testTicker'}");
            Dictionary<string, object>? StockPriceQuote = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(@"{'c':'1000'}");
            IOptions<TradingOptions> tradingOptions =  Options.Create(new TradingOptions() { DefaultOrderQuantity=100});
            string token=_fixture.Create<string>();         
            TradeController tradeController=new TradeController(_finnhubGetService, _finnhubSearchService, _stocksBuyOrderService, _stocksSellOrderService, tradingOptions, _configuration,_logger);
            //Mock
            _finnhubGetServiceMock
                .Setup(temp=> temp.GetCompanyProfile(It.IsAny<string>()))
                .ReturnsAsync(CompanyProfile);

            _finnhubGetServiceMock
                .Setup(temp=> temp.GetStockPriceQuote(It.IsAny<string>()))
                .ReturnsAsync(StockPriceQuote);
            
            _configurationMock
                .Setup(temp => temp["Token"])
                .Returns(token);

            StockTrade stockTrade = _fixture
                .Build<StockTrade>()
                .With(temp=> temp.Quantity, tradingOptions.Value.DefaultOrderQuantity)
                .With(temp=> temp.StockName, CompanyProfile["name"])
                .With(temp=> temp.StockSymbol, CompanyProfile["ticker"] )
                .With(temp=> temp.Price, StockPriceQuote["c"])
                .Create();
                
            //Act
            IActionResult results = await tradeController.Index(string.Empty);

            //Assert
            ViewResult viewResult =Assert.IsAssignableFrom<ViewResult>(results);
            viewResult.ViewData.Model.Should().BeAssignableTo<StockTrade>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(stockTrade);
        }
    }
}

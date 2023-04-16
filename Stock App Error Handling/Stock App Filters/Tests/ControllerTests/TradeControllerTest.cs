
using AutoFixture;
using Moq;
using FluentAssertions;
using Services;
using Stock_App;
using Stock_App.Controllers;
using Microsoft.Extensions.Options;
using ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Stock_App.Models;

namespace Tests.ControllerTests
{
    public class TradeControllerTest
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly IConfiguration _configuration;
        private readonly Mock <IFinnhubService> _finnhubServiceMock;
        private readonly Mock <IStocksService> _stocksServiceMock;
        private readonly Mock <IConfiguration> _configurationMock;
        private readonly Fixture _fixture;

       public TradeControllerTest()
        {
            _finnhubServiceMock = new Mock<IFinnhubService>();
            _stocksServiceMock = new Mock<IStocksService>();
            _configurationMock = new Mock<IConfiguration>();
            _finnhubService = _finnhubServiceMock.Object;
            _stocksService = _stocksServiceMock.Object;
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
            TradeController tradeController=new TradeController(_finnhubService, _stocksService, tradingOptions, _configuration);
            //Mock
            _finnhubServiceMock
                .Setup(temp=> temp.GetCompanyProfile(It.IsAny<string>()))
                .ReturnsAsync(CompanyProfile);

            _finnhubServiceMock
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

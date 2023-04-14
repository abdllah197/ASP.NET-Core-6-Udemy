using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.IntegrationTests
{
    public class TradeControllerIntegrationTest:IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region Index

        [Fact]
        public async Task Index_ToReturnView()
        {
            //Arrange

            //Act
            HttpResponseMessage responseMessage = await _httpClient.GetAsync("/Trade/Index/MSFT");

            //Assert
            responseMessage.Should().BeSuccessful();
            string responseBody= await responseMessage.Content.ReadAsStringAsync();
            HtmlDocument html=new HtmlDocument();
            html.LoadHtml(responseBody);
            var document = html.DocumentNode;
            document.QuerySelectorAll(".price").Should().NotBeNull();

        }

        #endregion
    }
}

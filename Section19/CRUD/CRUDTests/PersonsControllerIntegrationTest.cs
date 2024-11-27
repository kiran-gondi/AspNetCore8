using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CRUDTests
{
    public class PersonsControllerIntegrationTest : 
        IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PersonsControllerIntegrationTest(
            CustomWebApplicationFactory customWebApplicationFactory)
        {
            _client = customWebApplicationFactory.CreateClient();    
        }

        #region Index
        [Fact]
        public async void Index_ToReturnView()
        {
            //Arrange

            //Act
            HttpResponseMessage response = await _client.GetAsync("/Persons/Index");

            //ASsert
            response.Should().BeSuccessful();//2xx

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(responseBody);
            var document = htmlDocument.DocumentNode;

            document.QuerySelectorAll("table.persons").Should().NotBeNull();
        }

        #endregion
    }
}

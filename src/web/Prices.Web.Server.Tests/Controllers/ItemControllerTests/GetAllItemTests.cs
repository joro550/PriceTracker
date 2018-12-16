using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Blazor;
using Prices.Web.Server.Handlers.Data.Entities;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models.Home;
using Xunit;

namespace Prices.Web.Server.Tests.Controllers.ItemControllerTests
{
    public class GetAllItemTests : IClassFixture<WebApplicationFixture>
    {
        private readonly WebApplicationBuilder _applicationBuilder;

        public GetAllItemTests(WebApplicationFixture webApplicationFixture) 
            => _applicationBuilder = webApplicationFixture.ApplicationBuilder;
        
        
        [Theory, AutoData]
        public async Task WhenItemsExist_ThenItemsAreReturned(List<ItemEntity> items)
        {
            var application = _applicationBuilder
                .WithItemRepository(FakeItemRepository.WithItems(items))
                .Build();

            var response = await application.GetJsonAsync<List<Item>>("/api/items");
            Assert.NotNull(response);
            Assert.Equal(items.Count, response.Count);
            response.Should().BeEquivalentTo(items, cfg => cfg.Including(m => m.Id)
                .Including(m => m.Category)
                .Including(m => m.Retailer));
        }

        [Fact]
        public async Task WhenNoItemsExistInTheStore_NoContentResponseIsReturned()
        {
            var application = _applicationBuilder
                .WithItemRepository(FakeItemRepository.WithNoItems())
                .Build();
            
            var response = await application.GetAsync("/api/items");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
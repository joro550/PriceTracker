using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Prices.Web.Client.Tests.Fakes;
using Prices.Web.Shared.Models.Home;
using Xunit;

namespace Prices.Web.Client.Tests.Pages.Items
{
    public class ListItemComponentTests
    {
        private readonly ListItemComponentBuilder _componentBuilder;

        public ListItemComponentTests() 
            => _componentBuilder = new ListItemComponentBuilder();

        [Theory]
        [AutoData]
        public async Task WhenItemsAreReturnedFromRequest_ThenItemsGetSetToEmptyList(List<Item> items)
        {
            var component = _componentBuilder
                .WithMessageHandler(FakeHttpMessageHandler.WithResult(items))
                .Build();
            await component.InitAsync();
            component.GetItems().Should().BeEquivalentTo(items);
        }

        [Fact]
        public async Task WhenInitializingComponent_RequestForAllItemsIsMade()
        {
            var messageHandler = FakeHttpMessageHandler.WithNotFoundResult();
            var component = _componentBuilder
                .WithMessageHandler(messageHandler)
                .Build();
            await component.InitAsync();
            Assert.Contains("/api/items", messageHandler.GetRequests());
        }

        [Fact]
        public async Task WhenNotFoundIsReturned_ThenItemsGetSetToEmptyList()
        {
            var component = _componentBuilder.Build();
            await component.InitAsync();
            Assert.Empty(component.GetItems());
        }
    }
}
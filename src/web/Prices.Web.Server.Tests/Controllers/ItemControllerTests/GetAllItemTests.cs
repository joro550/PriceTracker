using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Handlers.Data.Entities;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models.Home;
using Xunit;

namespace Prices.Web.Server.Tests.Controllers.ItemControllerTests
{
    public class GetAllItemTests : IClassFixture<ItemControllerFixture>
    {
        public GetAllItemTests(ItemControllerFixture fixture)
        {
            _fixture = fixture.Builder;
        }

        private readonly ItemControllerBuilder _fixture;

        [Theory]
        [AutoData]
        public async Task WhenItemsExist_ThenItemsAreReturned(List<ItemEntity> items)
        {
            var itemController = _fixture
                .WithItemRepository(FakeItemRepository.WithItems(items))
                .Build();

            var result = await itemController.GetAllItems();
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<Item>>(objectResult.Value);

            Assert.Equal(items.Count, model.Count);
            model.Should().BeEquivalentTo(items, cfg => cfg.Including(m => m.Id)
                .Including(m => m.Category)
                .Including(m => m.Retailer));
        }

        [Fact]
        public async Task WhenNoRecordsToReturn_ThenNotFoundIsReturned()
        {
            var itemController = _fixture
                .WithItemRepository(FakeItemRepository.WithNoItems())
                .Build();

            var result = await itemController.GetAllItems();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task WhenStandardItemsExist_ThenItemsAreReturned()
        {
            var itemController = _fixture
                .WithItemRepository(FakeItemRepository.WithStandardItems())
                .Build();

            var result = await itemController.GetAllItems();
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<List<Item>>(objectResult.Value);

            Assert.Equal(FakeItemRepository.StandardItems.Count, model.Count);
            model.Should().BeEquivalentTo(FakeItemRepository.StandardItems,
                cfg => cfg.Including(m => m.Id)
                    .Including(m => m.Category)
                    .Including(m => m.Retailer));
        }
    }
}
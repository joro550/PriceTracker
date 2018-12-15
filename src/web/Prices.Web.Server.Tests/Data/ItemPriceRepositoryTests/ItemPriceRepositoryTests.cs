using System;
using System.Threading.Tasks;
using Prices.Web.Server.Handlers.Data.Entities;
using Xunit;

namespace Prices.Web.Server.Tests.Data.ItemPriceRepositoryTests
{
    public class ItemPriceRepositoryTests
    {
        private static ItemPriceEntity CreateItemPrice(string partitionKey)
        {
            return new ItemPriceEntity
            {
                PartitionKey = partitionKey,
                RowKey = $"{Guid.NewGuid():N}",
                PriceDate = DateTime.UtcNow
            };
        }

        public class GetAllTests : IClassFixture<ItemPriceRepositoryFixtureFactory>, IDisposable
        {
            public GetAllTests(ItemPriceRepositoryFixtureFactory fixtureFactory)
            {
                _fixture = fixtureFactory.Build();
            }

            public void Dispose()
            {
                _fixture.Dispose();
            }

            private readonly ItemPriceRepositoryFixture _fixture;

            [Fact]
            public async Task WhenThereAreItemsInTheTable_ItemsAreReturned()
            {
                var item = CreateItemPrice("b07");
                await _fixture.Repository.Add(item);

                var results = await _fixture.Repository.GetAll();
                Assert.Single(results);
                Assert.Equal(item.PartitionKey, results[0].PartitionKey);
                Assert.Equal(item.RowKey, results[0].RowKey);
            }

            [Fact]
            public async Task WhenThereAreNoItemsInTheTable_EmptyCollectionIsReturned()
            {
                Assert.Empty(await _fixture.Repository.GetAll());
            }
        }

        public class GetByPartitionKeyTests : IClassFixture<ItemPriceRepositoryFixtureFactory>, IDisposable
        {
            public GetByPartitionKeyTests(ItemPriceRepositoryFixtureFactory fixtureFactory)
            {
                _fixture = fixtureFactory.Build();
            }

            public void Dispose()
            {
                _fixture.Dispose();
            }

            private readonly ItemPriceRepositoryFixture _fixture;

            [Fact]
            public async Task WhenThereAreItemsWithSearchedPartitionKey_ThenItemPriceIsReturned()
            {
                const string partitionKey = "b07";
                var item = CreateItemPrice(partitionKey);
                await _fixture.Repository.Add(item);

                var results = await _fixture.Repository.ByPartitionKey(partitionKey);
                Assert.NotEmpty(results);
                Assert.Equal(item.PartitionKey, results[0].PartitionKey);
                Assert.Equal(item.RowKey, results[0].RowKey);
            }

            [Fact]
            public async Task WhenThereAreNoItemsInTheTable_EmptyCollectionIsReturned()
            {
                Assert.Empty(await _fixture.Repository.ByPartitionKey(string.Empty));
            }

            [Fact]
            public async Task WhenThereAreNoItemsWithTheSearchedPartitionKey_ThenEmptyCollectionIsReturned()
            {
                var item = CreateItemPrice("b07");
                await _fixture.Repository.Add(item);

                var results = await _fixture.Repository.ByPartitionKey("0");
                Assert.Empty(results);
            }
        }
    }
}
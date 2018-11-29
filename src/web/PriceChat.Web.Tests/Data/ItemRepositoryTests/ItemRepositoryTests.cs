using System;
using System.Threading.Tasks;
using PriceChat.Web.Data;
using Xunit;

namespace PriceChat.Web.Tests.Data.ItemRepositoryTests
{
    public class ItemRepositoryTests 
    {
        public class GetAllTests : IClassFixture<ItemRepositoryFixtureFactory>, IDisposable
        {
            private readonly ItemRepositoryFixture _fixture;

            public GetAllTests(ItemRepositoryFixtureFactory factory) 
                => _fixture = factory.Build();

            public void Dispose() 
                => _fixture?.Dispose();

            [Fact]
            public async Task WhenThereAreNoItemsInTheTable_EmptyCollectionIsReturned() 
                => Assert.Empty(await _fixture.Repository.GetAll());

            [Fact]
            public async Task WhenThereAreItemsInTheTable_ItemsAreReturned()
            {
                var item = CreateItem("Product");
                await _fixture.Repository.Add(item);

                var results = await _fixture.Repository.GetAll();
                Assert.Single(results);
                Assert.Equal(item.PartitionKey, results[0].PartitionKey);
                Assert.Equal(item.RowKey, results[0].RowKey);
                Assert.Equal(item.Category, results[0].Category);
                Assert.Equal(item.Id, results[0].Id);
                Assert.Equal(item.Retailer, results[0].Retailer);
            }
        }

        public class GetByPartitionKeyTests : IClassFixture<ItemRepositoryFixtureFactory>, IDisposable
        {
            private readonly ItemRepositoryFixture _fixture;

            public GetByPartitionKeyTests(ItemRepositoryFixtureFactory fixture) 
                => _fixture = fixture.Build();

            public void Dispose()
                => _fixture?.Dispose();

            [Fact]
            public async Task WhenThereAreNoItemsInTheTable_EmptyCollectionIsReturned()
                => Assert.Empty(await _fixture.Repository.ByPartitionKey(string.Empty));

            [Fact]
            public async Task WhenNoItemsMatchPartitionKey_EmptyCollectionIsReturned()
            {
                var item = CreateItem("Product");
                await _fixture.Repository.Add(item);

                var results = await _fixture.Repository.ByPartitionKey("NotProduct");
                Assert.Empty(results);
            }

            [Fact]
            public async Task WhenItemPartitionKeyMatchesQuery_ItemIsReturned()
            {
                const string partitionKey = "Product";
                var item = CreateItem(partitionKey);
                await _fixture.Repository.Add(item);

                var results = await _fixture.Repository.ByPartitionKey(partitionKey);
                Assert.Single(results);
                Assert.Equal(item.PartitionKey, results[0].PartitionKey);
                Assert.Equal(item.RowKey, results[0].RowKey);
                Assert.Equal(item.Category, results[0].Category);
                Assert.Equal(item.Id, results[0].Id);
                Assert.Equal(item.Retailer, results[0].Retailer);
            }
        }

        private static Item CreateItem(string partitionKey) => new Item
        {
            Id = "B07",
            RowKey = "B07",
            Category = "Game",
            Retailer = "Amazon",
            PartitionKey = partitionKey
        };
    }
}
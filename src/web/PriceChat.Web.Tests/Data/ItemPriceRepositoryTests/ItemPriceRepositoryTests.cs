using System;
using System.Threading.Tasks;
using PriceChat.Web.Data;
using Xunit;

namespace PriceChat.Web.Tests.Data.ItemPriceRepositoryTests
{
    public class ItemPriceRepositoryTests
    {
        public class GetAllTests : IClassFixture<ItemPriceRepositoryFixtureFactory>, IDisposable
        {
            private readonly ItemPriceRepositoryFixture _fixture;

            public GetAllTests(ItemPriceRepositoryFixtureFactory fixtureFactory) 
                => _fixture = fixtureFactory.Build();

            public void Dispose() 
                => _fixture.Dispose();
            
            [Fact]
            public async Task WhenThereAreNoItemsInTheTable_EmptyCollectionIsReturned() 
                => Assert.Empty(await _fixture.Repository.GetAll());

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
        }
        
        public class GetByPartitionKeyTests : IClassFixture<ItemPriceRepositoryFixtureFactory>, IDisposable
        {
            private readonly ItemPriceRepositoryFixture _fixture;

            public GetByPartitionKeyTests(ItemPriceRepositoryFixtureFactory fixtureFactory) 
                => _fixture = fixtureFactory.Build();

            public void Dispose() 
                => _fixture.Dispose();

            [Fact]
            public async Task WhenThereAreNoItemsInTheTable_EmptyCollectionIsReturned()
                => Assert.Empty(await _fixture.Repository.ByPartitionKey(string.Empty));

            [Fact]
            public async Task WhenThereAreNoItemsWithTheSearchedPartitionKey_ThenEmptyCollectionIsReturned()
            {
                var item = CreateItemPrice("b07");
                await _fixture.Repository.Add(item);

                var results = await _fixture.Repository.ByPartitionKey("0");
                Assert.Empty(results);
            }
            
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
        }

        private static ItemPrice CreateItemPrice(string partitionKey) => new ItemPrice
        {
            PartitionKey = partitionKey,
            RowKey = $"{Guid.NewGuid():N}"
        };
    }
}
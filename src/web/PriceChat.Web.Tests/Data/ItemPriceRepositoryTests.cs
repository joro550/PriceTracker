using System;
using System.Threading.Tasks;
using PriceChat.Web.Data;
using Xunit;

namespace PriceChat.Web.Tests.Data
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
                var item = CreateItemPrice();
                await _fixture.Repository.Add(item);

                var results = await _fixture.Repository.GetAll();
                Assert.Single(results);
                Assert.Equal(item.PartitionKey, results[0].PartitionKey);
                Assert.Equal(item.RowKey, results[0].RowKey);
            }

            private ItemPrice CreateItemPrice() => new ItemPrice
            {
                PartitionKey = "b07",
                RowKey = $"{Guid.NewGuid():N}"
            };
        }        
    }
}
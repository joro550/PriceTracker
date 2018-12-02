using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using PriceFinder.Models;
using PriceFinder.Tests.Fakes;
using Xunit;

namespace PriceFinder.Tests
{
    public class GetAmazonPriceFunctionTests : IAsyncLifetime
    {
        private CloudTable _tableReference;

        public async Task InitializeAsync()
        {
            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;

            var cloudTableClient = storageAccount.CreateCloudTableClient();
            _tableReference = cloudTableClient.GetTableReference($"tbl{Guid.NewGuid():N}");
            await _tableReference.CreateIfNotExistsAsync();
        }

        public async Task DisposeAsync() 
            => await _tableReference.DeleteAsync();

        [Theory]
        [InlineData("B0725VRJ6J", "£47.99", "ItemOnSale")]
        [InlineData("B005KK88CM", "£173.00", "AmazonOnSale")]
        [InlineData("B01C45OD6K", "£39.99", "DvdPage")]
        [InlineData("B07HWX2M9H", "£1368.28", "ItemWithCommasInPrice")]
        public async Task GivenAnItemId_ThenCorrectPriceIsEnteredIntoTheDatabase(string id, string expectedPrice, string typeOfItem)
        {
            var queueItem = new QueueItem {Id = id };
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(queueItem));

            GetAmazonPriceFunction.Client = new HttpClient(new FileLoaderMessageHandler(typeOfItem, "Amazon"));
            await GetAmazonPriceFunction.Run(message, _tableReference);

            var query = new TableQuery<ItemPrice>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));
            var result = await _tableReference.ExecuteQuerySegmentedAsync(query, null);
            var itemPrice = result.Results.First();

            Assert.NotNull(itemPrice);
            Assert.Equal(expectedPrice, itemPrice.Price);
            Assert.Equal(DateTime.UtcNow, itemPrice.PriceTime, TimeSpan.FromSeconds(2));
        }

        [Theory]
        [InlineData("B0725VRJ6J", "ItemOnSale")]
        public async Task GivenAnItemId_ThenCorrectRequestIsMadeToRetrieveItem(string id, string typeOfItem)
        {
            var queueItem = new QueueItem { Id = id };
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(queueItem));

            var fileLoaderMessageHandler = new FileLoaderMessageHandler(typeOfItem, "Amazon");
            GetAmazonPriceFunction.Client = new HttpClient(fileLoaderMessageHandler);
            await GetAmazonPriceFunction.Run(message, _tableReference);

            var requests = fileLoaderMessageHandler.GetRequests();
            Assert.Contains($"https://www.amazon.co.uk/dp/{id}", requests);
        }
    }
}
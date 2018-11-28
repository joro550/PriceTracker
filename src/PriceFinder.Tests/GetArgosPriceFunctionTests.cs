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
    public class GetArgosPriceFunctionTests : IAsyncLifetime
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
            => await _tableReference.DeleteIfExistsAsync();

        [Fact]
        public async Task GivenAnItemId_ThenCorrectPriceIsEnteredIntoTheDatabase()
        {
            const string itemId = "8665454";

            var queueItem = new QueueItem {Id = itemId };
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(queueItem));

            GetArgosPriceFunction.Client = new HttpClient(new FileLoaderMessageHandler("Item", "Argos"));
            await GetArgosPriceFunction.Run(message, _tableReference);

            var query = new TableQuery<ItemPrice>().Where($"PartitionKey eq '{itemId}'");
            var result = await _tableReference.ExecuteQuerySegmentedAsync(query, null);
            var itemPrice = result.Results.First();

            Assert.NotNull(itemPrice);
            Assert.Equal("£199.99", itemPrice.Price);
        }

        [Fact]
        public async Task GivenAnItemId_ThenCorrectRequestIsMadeToRetrieveItem()
        {
            const string itemId = "8665454";

            var queueItem = new QueueItem { Id = itemId };
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(queueItem));

            var fileLoaderMessageHandler = new FileLoaderMessageHandler("Item", "Argos");
            GetArgosPriceFunction.Client = new HttpClient(fileLoaderMessageHandler);
            await GetArgosPriceFunction.Run(message, _tableReference);

            var requests = fileLoaderMessageHandler.GetRequests();
            Assert.Contains($"https://www.argos.co.uk/product/{itemId}", requests);
        }
    }
}
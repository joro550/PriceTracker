using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PriceFinder.Models;
using PriceFinder.Tests.Extensions;
using PriceFinder.Tests.Stubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;
using Xunit;

namespace PriceFinder.Tests
{
    public class GetItemTests : IAsyncLifetime
    {
        private readonly CloudStorageAccount _storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
        private CloudTable _tableReference;

        public async Task InitializeAsync()
        {
            _tableReference = _storageAccount
                .CreateCloudTableClient()
                .GetTableReference($"tbl{Guid.NewGuid():N}");

            await _tableReference.CreateIfNotExistsAsync();
        }

        public async Task DisposeAsync()
        {
            await _tableReference.DeleteIfExistsAsync();

            var queueClient = _storageAccount.CreateCloudQueueClient();
            var queueSegment = await queueClient.ListQueuesSegmentedAsync(new QueueContinuationToken());

            foreach (var queue in queueSegment.Results)
                await queue.DeleteIfExistsAsync();
        }

        [Theory]
        [InlineData("B07Q", "Amazon")]
        [InlineData("B07P", "Amazon")]
        [InlineData("B07L", "Amazon")]
        public async Task GivenADatabaseRecord_ThenItemGetsPutOnRetailerBasedQueue(string identifier, string retailer)
        {
            await AddItem(identifier, retailer);
            await GetItemsFunctions.Run(new TimerStub(), _tableReference, _storageAccount, new StubLogger());

            var queueName = $"{retailer}-item-queue".ToLower();
            var queueClient = _storageAccount.CreateCloudQueueClient();
            var queueReference = queueClient.GetQueueReference(queueName);

            var firstMessage = await queueReference.GetMessageAs<QueueItem>();
            Assert.Equal(identifier, firstMessage.Id);
        }

        [Fact]
        public async Task GivenAMultipleItemsInTheDatabaseWhenRunningTheFunction_ItemIsAddedToTheQueue()
        {
            var identifiers = new List<string> { "B07Q", "B07P", "B07L" };

            foreach (var identifier in identifiers)
                await AddItem(identifier);

            await GetItemsFunctions.Run(new TimerStub(), _tableReference, _storageAccount, new StubLogger());

            var queueClient = _storageAccount.CreateCloudQueueClient();
            var queueReference = queueClient.GetQueueReference("amazon-item-queue");

            var messages = await queueReference.GetMessagesAs<QueueItem>(3);
            Assert.Contains(messages, message => message.Id == "B07Q");
            Assert.Contains(messages, message => message.Id == "B07P");
            Assert.Contains(messages, message => message.Id == "B07L");
        }

//        [Fact()]
//        public async Task GivenANoDataInTheDatabaseWhenRunningTheFunction_ItemIsAddedToTheQueue()
//        {
//            await GetItemsFunctions.Run(new TimerStub(), _tableReference, _storageAccount, new StubLogger());
//            Assert.Null(await _queueReference.GetMessageAsync());
//        }

        private async Task<TableResult> AddItem(string itemIdentifier, string retailer = "Amazon") => await _tableReference.ExecuteAsync(TableOperation.Insert(new Item
        {
            PartitionKey = "Product",
            RowKey = itemIdentifier,
            Id = itemIdentifier,
            Category = "Game",
            Retailer = retailer
        }));
    }
}

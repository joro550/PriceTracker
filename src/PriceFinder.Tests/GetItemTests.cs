using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using PriceFinder.Models;
using PriceFinder.Tests.Extensions;
using PriceFinder.Tests.Stubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PriceFinder.Tests
{
    public class GetItemTests : IAsyncLifetime
    {
        private CloudTable _tableReference;
        private CloudQueue _queueReference;

        public async Task InitializeAsync()
        {
            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;

            var cloudTableClient = storageAccount.CreateCloudTableClient();
            var cloudQueueClient = storageAccount.CreateCloudQueueClient();

            _queueReference = cloudQueueClient.GetQueueReference($"queue{Guid.NewGuid():N}");
            await _queueReference.CreateIfNotExistsAsync();

            _tableReference = cloudTableClient.GetTableReference($"tbl{Guid.NewGuid():N}");
            await _tableReference.CreateIfNotExistsAsync();
        }

        public async Task DisposeAsync()
        {
            await _queueReference.DeleteIfExistsAsync();
            await _tableReference.DeleteIfExistsAsync();
        }

        [Theory]
        [InlineData("B07Q")]
        [InlineData("B07P")]
        [InlineData("B07L")]
        public async Task GivenASingleItemInTheDatabaseWhenRunningTheFunction_ItemIsAddedToTheQueue(string identifier)
        {
            await AddItem(identifier);
            await GetItemsFunctions.Run(new TimerStub(), _tableReference, _queueReference, new StubLogger());

            var firstMessage = await _queueReference.GetMessageAs<QueueItem>();
            Assert.Equal(identifier, firstMessage.Id);
        }

        [Fact]
        public async Task GivenAMultipleItemsInTheDatabaseWhenRunningTheFunction_ItemIsAddedToTheQueue()
        {
            var identifiers = new List<string> { "B07Q", "B07P", "B07L" };

            foreach (var identifier in identifiers)
                await AddItem(identifier);

            await GetItemsFunctions.Run(new TimerStub(), _tableReference, _queueReference, new StubLogger());

            var messages = await _queueReference.GetMessagesAs<QueueItem>(3);
            Assert.Contains(messages, message => message.Id == "B07Q");
            Assert.Contains(messages, message => message.Id == "B07P");
            Assert.Contains(messages, message => message.Id == "B07L");
        }

        [Fact]
        public async Task GivenANoDataInTheDatabaseWhenRunningTheFunction_ItemIsAddedToTheQueue()
        {
            await GetItemsFunctions.Run(new TimerStub(), _tableReference, _queueReference, new StubLogger());
            Assert.Null(await _queueReference.GetMessageAsync());
        }

        private async Task<TableResult> AddItem(string itemIdentifier)
        {
            return await _tableReference.ExecuteAsync(TableOperation.Insert(new Item
            {
                PartitionKey = "Product",
                RowKey = $"{itemIdentifier}",
                Id = itemIdentifier,
                Category = "Game",
                Retailer = "Amazon"
            }));
        }
    }
}

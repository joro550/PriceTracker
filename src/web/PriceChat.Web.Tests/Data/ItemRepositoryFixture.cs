using System;
using PriceChat.Web.Data;
using PriceChat.Web.Tests.Fakes;

namespace PriceChat.Web.Tests.Data
{
    public class ItemRepositoryFixture : IDisposable
    {
        private readonly FakeTableStorageClient _tableClient;
        public ItemRepository Repository { get; }

        public ItemRepositoryFixture()
        {
            _tableClient = FakeStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient();
            Repository = new ItemRepository(_tableClient);
        }

        public void Dispose()
            => _tableClient.DeleteCreatedTables();
    }
}
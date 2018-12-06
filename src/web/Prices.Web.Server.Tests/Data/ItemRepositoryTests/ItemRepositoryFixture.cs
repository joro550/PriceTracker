using System;
using Prices.Web.Server.Data;
using Prices.Web.Server.Tests.Fakes;

namespace Prices.Web.Server.Tests.Data.ItemRepositoryTests
{
    public class ItemRepositoryFixture : IDisposable
    {
        private readonly FakeTableStorageClient _tableClient;

        public ItemRepositoryFixture()
        {
            _tableClient = FakeStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient();
            Repository = new ItemRepository(_tableClient);
        }

        public ItemRepository Repository { get; }

        public void Dispose()
        {
            _tableClient.DeleteCreatedTables();
        }
    }
}
using System;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Tests.Fakes;

namespace Prices.Web.Server.Tests.Data.ItemPriceRepositoryTests
{
    public class ItemPriceRepositoryFixture : IDisposable
    {
        private readonly FakeTableStorageClient _tableClient;

        public ItemPriceRepositoryFixture()
        {
            _tableClient = FakeStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient();
            Repository = new ItemPriceRepository(_tableClient);
        }

        public ItemPriceRepository Repository { get; }

        public void Dispose()
        {
            _tableClient.DeleteCreatedTables();
        }
    }
}
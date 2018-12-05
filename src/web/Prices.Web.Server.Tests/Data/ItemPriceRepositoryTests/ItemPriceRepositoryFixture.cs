using System;
using Prices.Web.Server.Data;
using Prices.Web.Server.Tests.Fakes;

namespace Prices.Web.Server.Tests.Data.ItemPriceRepositoryTests
{
    public class ItemPriceRepositoryFixture : IDisposable
    {
        private readonly FakeTableStorageClient _tableClient;
        public ItemPriceRepository Repository { get; }

        public ItemPriceRepositoryFixture()
        {
            _tableClient = FakeStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient();
            Repository = new ItemPriceRepository(_tableClient);
        }

        public void Dispose()
            => _tableClient.DeleteCreatedTables();
    }
}
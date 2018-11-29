using System;
using PriceChat.Web.Data;
using PriceChat.Web.Tests.Fakes;

namespace PriceChat.Web.Tests.Data.ItemPriceRepositoryTests
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
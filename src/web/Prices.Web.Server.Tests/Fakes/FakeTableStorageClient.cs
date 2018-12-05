using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeTableStorageClient : CloudTableClient
    {
        private readonly CloudTableClient _tableClient;
        private readonly List<string> _tablesCreated = new List<string>();

        public FakeTableStorageClient(CloudTableClient tableClient, StorageUri baseUri, StorageCredentials credentials)
            : base(baseUri, credentials)
        {
            _tableClient = tableClient;
        }

        public override CloudTable GetTableReference(string originalTableName)
        {
            var generatedTableName = $"tbl{Guid.NewGuid():N}{originalTableName}";
            _tablesCreated.Add(generatedTableName);

            var tableReference = _tableClient.GetTableReference(generatedTableName);
            Task.Run(async () => await tableReference.CreateIfNotExistsAsync())
                .Wait();

            return tableReference;
        }

        public void DeleteCreatedTables() => Task.Run(async () =>
        {
            foreach (var createdTable in _tablesCreated)
            {
                var tableReference = _tableClient.GetTableReference(createdTable);
                await tableReference.DeleteIfExistsAsync();

            }
        }).Wait();
    }
}
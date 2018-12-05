using Microsoft.WindowsAzure.Storage;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeStorageAccount
    {
        private readonly CloudStorageAccount _storageAccount;

        public static FakeStorageAccount DevelopmentStorageAccount
            => new FakeStorageAccount(CloudStorageAccount.DevelopmentStorageAccount);

        private FakeStorageAccount(CloudStorageAccount storageAccount) 
            => _storageAccount = storageAccount;

        public FakeTableStorageClient CreateCloudTableClient()
            => new FakeTableStorageClient(_storageAccount.CreateCloudTableClient(),
                _storageAccount.TableStorageUri, _storageAccount.Credentials);
    }
}
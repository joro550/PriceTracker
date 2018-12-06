using Microsoft.WindowsAzure.Storage;

namespace Prices.Web.Server.Tests.Fakes
{
    public class FakeStorageAccount
    {
        private readonly CloudStorageAccount _storageAccount;

        private FakeStorageAccount(CloudStorageAccount storageAccount)
        {
            _storageAccount = storageAccount;
        }

        public static FakeStorageAccount DevelopmentStorageAccount
            => new FakeStorageAccount(CloudStorageAccount.DevelopmentStorageAccount);

        public FakeTableStorageClient CreateCloudTableClient()
        {
            return new FakeTableStorageClient(_storageAccount.CreateCloudTableClient(),
                _storageAccount.TableStorageUri, _storageAccount.Credentials);
        }
    }
}
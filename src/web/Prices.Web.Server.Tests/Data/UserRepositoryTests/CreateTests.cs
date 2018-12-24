using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Data.Entities;
using Prices.Web.Server.Tests.Fakes;
using Xunit;

namespace Prices.Web.Server.Tests.Data.UserRepositoryTests
{
    public class CreateTests : IDisposable
    {
        public CreateTests()
        {
            _fakeTableStorageClient = FakeStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient();
        }

        public void Dispose()
        {
            _fakeTableStorageClient.DeleteCreatedTables();
        }

        private readonly FakeTableStorageClient _fakeTableStorageClient;

        private static UserEntity CreateUser(string username, string id = "B07")
        {
            return new UserEntity
            {
                Id = id,
                PartitionKey = "B07",
                RowKey = "B07",
                Username = username,
                Password = "password"
            };
        }


        [Fact]
        public async Task WhenStoreHasRecords_ThenUserIsAdded()
        {
            var userRepository = new UserRepository(_fakeTableStorageClient);
            var user = CreateUser("username");
            await userRepository.Add(user);
            var usersFromStore = await userRepository.GetAll();
            var firstUser = usersFromStore.First();

            firstUser.Should().BeEquivalentTo(user, options => options
                .Including(prop => prop.Id)
                .Including(prop => prop.PartitionKey)
                .Including(prop => prop.Username)
                .Including(prop => prop.Password));
        }
    }
}
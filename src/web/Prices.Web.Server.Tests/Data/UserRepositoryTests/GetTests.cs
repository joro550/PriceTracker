using Xunit;
using System;
using System.Linq;
using FluentAssertions;
using AutoFixture.Xunit2;
using System.Threading.Tasks;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Server.Handlers.Data;
using Prices.Web.Server.Handlers.Data.Entities;

namespace Prices.Web.Server.Tests.Data.UserRepositoryTests
{
    public class GetTests
    {
        public class GetAllTests : IDisposable
        {
            private readonly FakeTableStorageClient _fakeTableStorageClient;

            public GetAllTests() => _fakeTableStorageClient = FakeStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient();

            public void Dispose() 
                => _fakeTableStorageClient.DeleteCreatedTables();

            [Fact]
            public async Task WhenStoreHasNoRecords_EmptyListIsReturned()
            {
                var userRepository = new UserRepository(_fakeTableStorageClient);
                Assert.Empty(await userRepository.GetAll());
            }

            [Fact]
            public async Task WhenStoreHasRecords_ListHasExpectedRecords()
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
        
        public class GetByUsernameTests : IDisposable
        {
            private readonly FakeTableStorageClient _fakeTableStorageClient;

            public GetByUsernameTests() => _fakeTableStorageClient = FakeStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient();

            public void Dispose() 
                => _fakeTableStorageClient.DeleteCreatedTables();

            [Fact]
            public async Task WhenStoreHasNoRecords_NullRecordIsReturned()
            {
                var userRepository = new UserRepository(_fakeTableStorageClient);
                var userEntity = await userRepository.GetByUsername(string.Empty);
                Assert.Null(userEntity);
            }

            [Fact]
            public async Task WhenStoreHasRecords_ExpectedRecordIsReturned()
            {
                var userRepository = new UserRepository(_fakeTableStorageClient);
                const string username = "username";
                var user = CreateUser(username);
                await userRepository.Add(user);
                var usersFromStore = await userRepository.GetByUsername(username);
                
                usersFromStore.Should().BeEquivalentTo(user, options => options
                    .Including(prop => prop.Id)
                    .Including(prop => prop.PartitionKey)
                    .Including(prop => prop.Username)
                    .Including(prop => prop.Password));
            }
        }
        
        public class GetByIdTests : IDisposable
        {
            private readonly FakeTableStorageClient _fakeTableStorageClient;

            public GetByIdTests() => _fakeTableStorageClient = FakeStorageAccount
                .DevelopmentStorageAccount
                .CreateCloudTableClient();

            public void Dispose() 
                => _fakeTableStorageClient.DeleteCreatedTables();

            [Fact]
            public async Task WhenStoreHasNoRecords_NullRecordIsReturned()
            {
                var userRepository = new UserRepository(_fakeTableStorageClient);
                var userEntity = await userRepository.GetById(string.Empty);
                Assert.Null(userEntity);
            }

            [Theory, AutoData]
            public async Task WhenStoreHasRecords_ExpectedRecordIsReturned(string id)
            {
                var userRepository = new UserRepository(_fakeTableStorageClient);
                var user = CreateUser("username", id);
                await userRepository.Add(user);
                var usersFromStore = await userRepository.GetById(id);
                
                usersFromStore.Should().BeEquivalentTo(user, options => options
                    .Including(prop => prop.Id)
                    .Including(prop => prop.PartitionKey)
                    .Including(prop => prop.Username)
                    .Including(prop => prop.Password));
            }
        }
        
        private static UserEntity CreateUser(string username, string id = "B07") => new UserEntity
        {
            Id = id,
            PartitionKey = "B07",
            RowKey = "B07",
            Username = username,
            Password = "password",
        };
    }
}
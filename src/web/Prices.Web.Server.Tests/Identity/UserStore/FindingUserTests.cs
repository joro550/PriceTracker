using System.Threading;
using System.Threading.Tasks;
using Prices.Web.Server.Tests.Fakes;
using Xunit;

namespace Prices.Web.Server.Tests.Identity.UserStore
{
    public class FindingUserTests
    {
        public class FindUserByIdAsyncTest
        {
            private readonly UserStoreBuilder _storeBuilder;

            public FindUserByIdAsyncTest() 
                => _storeBuilder = new UserStoreBuilder();

            [Fact]
            public async Task WhenStoreIsEmpty_NullIsReturned()
            {
                var userStore = _storeBuilder.Build();
                var userId = await userStore.FindByIdAsync("1", CancellationToken.None);
                Assert.Null(userId);
            }

            [Fact]
            public async Task WhenStoreHasValidUser_ThenIdIsReturned()
            {
                var userStore = _storeBuilder
                    .WithUserRepository(FakeUserRepository.WithDefaultUsers())
                    .Build();

                var user = await userStore.FindByIdAsync(FakeUserRepository.NormalUser.Id, CancellationToken.None);
                Assert.Equal(FakeUserRepository.NormalUser.Id, user.Id);
                Assert.Equal(FakeUserRepository.NormalUser.Username, user.UserName);
                Assert.Equal(FakeUserRepository.NormalUser.Password, user.Password);
            }
        }

        public class FindUserByNameAsyncTest
        {
            private readonly UserStoreBuilder _storeBuilder;

            public FindUserByNameAsyncTest()
                => _storeBuilder = new UserStoreBuilder();

            [Fact]
            public async Task WhenStoreIsEmpty_NullIsReturned()
            {
                var userStore = _storeBuilder.Build();
                var userId = await userStore.FindByNameAsync("userName", CancellationToken.None);
                Assert.Null(userId);
            }

            [Fact]
            public async Task WhenStoreHasValidUser_ThenIdIsReturned()
            {
                var userStore = _storeBuilder
                    .WithUserRepository(FakeUserRepository.WithDefaultUsers())
                    .Build();

                var user = await userStore.FindByNameAsync(FakeUserRepository.NormalUser.Username,
                    CancellationToken.None);
                Assert.Equal(FakeUserRepository.NormalUser.Id, user.Id);
                Assert.Equal(FakeUserRepository.NormalUser.Username, user.UserName);
                Assert.Equal(FakeUserRepository.NormalUser.Password, user.Password);
            }
        }
    }
}
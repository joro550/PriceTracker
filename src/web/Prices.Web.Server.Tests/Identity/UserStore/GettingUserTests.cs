using System.Threading;
using System.Threading.Tasks;
using Prices.Web.Server.Identity;
using Prices.Web.Server.Tests.Fakes;
using Xunit;

namespace Prices.Web.Server.Tests.Identity.UserStore
{
    public class GettingUserTests
    {
        private static UserStoreBuilder UserStoreBuilder { get; } = new UserStoreBuilder();

        [Fact]
        public void CanCreateStore() 
            => Assert.NotNull(UserStoreBuilder.Build());

        public class GettingAUserIdTests
        {
            [Fact]
            public async Task WhenStoreIsEmpty_NullIsReturned()
            {
                var userStore = UserStoreBuilder.Build();
                var userId = await userStore.GetUserIdAsync(new PriceWebUser(), CancellationToken.None);
                Assert.Null(userId);
            }

            [Fact]
            public async Task WhenStoreHasValidUser_ThenIdIsReturned()
            {
                var userStore = UserStoreBuilder
                    .WithUserRepository(FakeUserRepository.WithDefaultUsers())
                    .Build();
                
                var normalUser = FakeUserRepository.NormalUser;
                var priceWebUser = new PriceWebUser {UserName = normalUser.Username, Password = normalUser.Password};
                var userId = await userStore.GetUserIdAsync(priceWebUser, CancellationToken.None);
                Assert.Equal(FakeUserRepository.NormalUser.Id, userId);
            }
        }

        public class GettingAUserNameTests
        {
            [Fact]
            public async Task WhenStoreIsEmpty_NullIsReturned()
            {
                var userStore = UserStoreBuilder.Build();
                var userName = await userStore.GetUserNameAsync(new PriceWebUser(), CancellationToken.None);
                Assert.Null(userName);
            }

            [Fact]
            public async Task WhenStoreHasValidUser_ThenIdIsReturned()
            {
                var userStore = UserStoreBuilder
                    .WithUserRepository(FakeUserRepository.WithDefaultUsers())
                    .Build();

                var normalUser = FakeUserRepository.NormalUser;
                var priceWebUser = new PriceWebUser { UserName = normalUser.Username, Password = normalUser.Password };
                var userName = await userStore.GetUserNameAsync(priceWebUser, CancellationToken.None);
                Assert.Equal(FakeUserRepository.NormalUser.Username, userName);
            }
        }

        public class GettingANormalizedUserNameTests
        {
            [Fact]
            public async Task WhenStoreIsEmpty_NullIsReturned()
            {
                var userStore = UserStoreBuilder.Build();
                var userName = await userStore.GetNormalizedUserNameAsync(new PriceWebUser(), CancellationToken.None);
                Assert.Null(userName);
            }

            [Fact]
            public async Task WhenStoreHasValidUser_ThenIdIsReturned()
            {
                var userStore = UserStoreBuilder
                    .WithUserRepository(FakeUserRepository.WithDefaultUsers())
                    .Build();

                var normalUser = FakeUserRepository.NormalUser;
                var priceWebUser = new PriceWebUser { UserName = normalUser.Username, Password = normalUser.Password };
                var userName = await userStore.GetNormalizedUserNameAsync(priceWebUser, CancellationToken.None);
                Assert.Equal(FakeUserRepository.NormalUser.Username, userName);
            }
        }
    }
}
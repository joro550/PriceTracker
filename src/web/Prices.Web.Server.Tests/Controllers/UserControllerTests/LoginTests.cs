using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models.Users;
using Xunit;

namespace Prices.Web.Server.Tests.Controllers.UserControllerTests
{
    public class LoginTests : IClassFixture<WebApplicationFixture>
    {
        public LoginTests(WebApplicationFixture fixture)
        {
            _fixture = fixture.ApplicationBuilder;
        }

        private readonly WebApplicationBuilder _fixture;

        [Fact]
        public async Task WhenCorrectUserInformationIsGiven_ThenTokenWithOkStatusIsReturned()
        {
            var webApplication = _fixture.WithUserRepository(FakeUserRepository.WithDefaultUsers()).Build();
            var normalUser = FakeUserRepository.NormalUser;
            var response = await webApplication.PostAsJsonAsync("/api/user/login",
                new UserModel {Username = normalUser.Username, Password = normalUser.OriginalPassword});

            var tokenResult = JsonConvert.DeserializeObject<TokenResult>(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(tokenResult);
            Assert.NotEmpty(tokenResult.Token);
        }

        [Fact]
        public async Task WhenIncorrectUsernameIsPassed_ThenBadRequestIsReturned()
        {
            var webApplication = _fixture.WithUserRepository(FakeUserRepository.WithDefaultUsers()).Build();
            var response = await webApplication.PostAsJsonAsync("/api/user/login",
                new UserModel {Username = "WrongUsername", Password = "WrongPassword"});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Empty(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task WhenNoUsersExist_ThenBadRequestIsReturned()
        {
            var webApplication = _fixture.WithUserRepository(FakeUserRepository.WithNoRecords()).Build();
            var response = await webApplication.PostAsJsonAsync("/api/user/login",
                new UserModel {Username = "username", Password = "password"});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Empty(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task WhenPasswordIsNotSpecified_ThenBadRequestIsReturned()
        {
            var webApplication = _fixture.WithUserRepository(FakeUserRepository.WithDefaultUsers()).Build();
            var response = await webApplication.PostAsJsonAsync("/api/user/login",
                new UserModel {Username = "username"});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Empty(await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task WhenUsernameIsNotSpecified_ThenBadRequestIsReturned()
        {
            var webApplication = _fixture.WithUserRepository(FakeUserRepository.WithDefaultUsers()).Build();
            var response = await webApplication.PostAsJsonAsync("/api/user/login",
                new UserModel {Password = "WrongPassword"});

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Empty(await response.Content.ReadAsStringAsync());
        }
    }
}
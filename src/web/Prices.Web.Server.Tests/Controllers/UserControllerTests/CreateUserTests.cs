using Xunit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models.Users;

namespace Prices.Web.Server.Tests.Controllers.UserControllerTests
{
    public class CreateUserTests : IClassFixture<WebApplicationFixture>
    {
        private readonly WebApplicationBuilder _fixture;

        public CreateUserTests(WebApplicationFixture fixture)
            => _fixture = fixture.ApplicationBuilder;

        [Theory]
        [InlineData("", "")]
        [InlineData("username", "")]
        [InlineData("", "password")]
        public async Task WhenInformationIsMissing_ThenBadRequestIsReturned(string username, string password)
        {
            var webApplication = _fixture
                .WithUserRepository(FakeUserRepository.WithNoRecords())
                .Build();

            var createUserModel = new CreateUserModel
                {Username = username, Password = password, VerifyPassword = password};

            var response = await webApplication.PostAsJsonAsync("/api/user/create", createUserModel);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task WhenUsernameAllReadyExists_ThenBadRequestIsReturned()
        {
            var webApplication = _fixture
                .WithUserRepository(FakeUserRepository.WithDefaultUsers())
                .Build();

            var createUserModel = new CreateUserModel
            {
                Username = FakeUserRepository.NormalUser.Username,
                Password = FakeUserRepository.NormalUser.OriginalPassword,
                VerifyPassword = FakeUserRepository.NormalUser.OriginalPassword
            };
            var response = await webApplication.PostAsJsonAsync("/api/user/create", createUserModel);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
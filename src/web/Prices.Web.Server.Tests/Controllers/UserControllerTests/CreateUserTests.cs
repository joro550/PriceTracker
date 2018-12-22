using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation.Results;
using Newtonsoft.Json;
using Prices.Web.Server.Identity;
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
        public async Task WhenPasswordsDontMatch_ThenBadRequestIsReturned()
        {
            var webApplication = _fixture
                .WithUserRepository(FakeUserRepository.WithNoRecords())
                .Build();

            var createUserModel = new CreateUserModel
                { Username = "username", Password = "password1", VerifyPassword = "password2" };

            var response = await webApplication.PostAsJsonAsync("/api/user/create", createUserModel);
            var value = await response.Content.ReadAsStringAsync();
            var validationFailures = JsonConvert.DeserializeObject<List<ValidationFailure>>(value);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("'Verify Password' should be equal to 'password1'.", 
                validationFailures.Select(failure => failure.ErrorMessage));
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

        [Fact]
        public async Task WhenValidUserIsRequestedToBeAdded_ThenOkIsReturnedAndUserIsSavedToDatabase()
        {
            var userRepository = FakeUserRepository.WithNoRecords();
            var webApplication = _fixture
                .WithUserRepository(userRepository)
                .Build();

            var createUserModel = new CreateUserModel
            {
                Username = FakeUserRepository.NormalUser.Username,
                Password = FakeUserRepository.NormalUser.OriginalPassword,
                VerifyPassword = FakeUserRepository.NormalUser.OriginalPassword
            };

            var response = await webApplication.PostAsJsonAsync("/api/user/create", createUserModel);
            var user = await userRepository.GetByUsername(FakeUserRepository.NormalUser.Username);

            var cipher = new CipherService();
            var canValidatePassword = cipher.ValidatePasswordAgainstHash(FakeUserRepository.NormalUser.OriginalPassword,
                user.PasswordSalt, user.Password);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(canValidatePassword);

            Assert.NotNull(user);
            Assert.Equal(FakeUserRepository.NormalUser.Username, user.Username);
        }
    } 
}
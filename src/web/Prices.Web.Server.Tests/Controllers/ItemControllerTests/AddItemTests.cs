using Xunit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models.Items;

namespace Prices.Web.Server.Tests.Controllers.ItemControllerTests
{
    public class AddItemTests : IClassFixture<WebApplicationFixture>
    {
        private readonly WebApplicationBuilder _fixture;

        public AddItemTests(WebApplicationFixture fixture)
            => _fixture = fixture.ApplicationBuilder;

        [Fact]
        public async Task WhenAnUnauthorizedUserTriesToAddAnItem_ThenUnauthorizedIsReturned()
        {
            var webApplication = _fixture
                .WithUserRepository(FakeUserRepository.WithNoRecords())
                .WithItemRepository(FakeItemRepository.WithNoItems())
                .Build();

            var response = await webApplication.PostAsJsonAsync("/api/items/create", new AddItemModel());
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public class InvalidModelTests : IClassFixture<WebApplicationFixture>
        {
            private readonly HttpClient _webApplication;
            private Fixture _autoFixture;

            public InvalidModelTests(WebApplicationFixture fixture)
            {
                _autoFixture = new Fixture();

                _webApplication = fixture.ApplicationBuilder
                    .WithUserRepository(FakeUserRepository.WithDefaultUsers())
                    .WithItemRepository(FakeItemRepository.WithNoItems())
                    .Build()
                    .AuthorizeWith(FakeUserRepository.NormalUser);
            }

            [Fact]
            public async Task WhenItemModelIsEmpty_ThenBadRequestIsReturned()
            {
                var addItemModel = new AddItemModel();
                var response = await _webApplication
                    .PostAsJsonAsync("/api/items/create", addItemModel);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }
    }
}
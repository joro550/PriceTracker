using Xunit;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        [Fact]
        public async Task WhenItemModelIsNull_ThenBadRequestIsReturned()
        {
            var webApplication = _fixture
                .WithUserRepository(FakeUserRepository.WithDefaultUsers())
                .WithItemRepository(FakeItemRepository.WithNoItems())
                .Build();

            var response = await webApplication
                .AuthorizeWith(FakeUserRepository.NormalUser)
                .PostAsJsonAsync("/api/items/create", new AddItemModel());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
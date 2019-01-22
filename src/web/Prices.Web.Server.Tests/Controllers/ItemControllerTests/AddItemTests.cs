using Xunit;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation.Results;
using System.Collections.Generic;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models.Items;
using Prices.Web.Server.Handlers.Data;

namespace Prices.Web.Server.Tests.Controllers.ItemControllerTests
{
    public class AddItemTests : IClassFixture<WebApplicationFixture>
    {
        public AddItemTests(WebApplicationFixture fixture)
        {
            _fixture = fixture.ApplicationBuilder;
        }

        private readonly WebApplicationBuilder _fixture;

        private async Task<HttpResponseMessage> CreateItem(IItemRepository fakeItemRepository, AddItemModel itemModel)
        {
            return await _fixture
                .WithUserRepository(FakeUserRepository.WithDefaultUsers())
                .WithItemRepository(fakeItemRepository)
                .Build()
                .AuthorizeWith(FakeUserRepository.NormalUser)
                .PostAsJsonAsync("/api/items/create", itemModel);
        }

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
        public async Task WhenItemModelIsEmpty_ThenBadRequestIsReturned()
        {
            var fakeItemRepository = FakeItemRepository.WithNoItems();
            var response = await CreateItem(fakeItemRepository, new AddItemModel());

            var errors =
                JsonConvert.DeserializeObject<List<ValidationFailure>>(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Item Id is required", errors.Select(e => e.ErrorMessage));
            Assert.Contains("Item Retailer is required", errors.Select(e => e.ErrorMessage));
            Assert.Contains("Item Category is required", errors.Select(e => e.ErrorMessage));
        }

        [Fact]
        public async Task WhenRetailerIsUnexpectedValue_ThenBadRequestIsReturned()
        {
            var fakeItemRepository = FakeItemRepository.WithNoItems();
            var response = await CreateItem(fakeItemRepository, new AddItemModel
                {Id = "1", Category = "gaming", Retailer = "NotARetailer"});

            var errors =
                JsonConvert.DeserializeObject<List<ValidationFailure>>(await response.Content.ReadAsStringAsync());
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Please specify a known retailer", errors.Select(e => e.ErrorMessage));
        }

        [Fact]
        public async Task WhenValidItemIsAdded_ThenItemIsAddedInTheDatabase()
        {
            const string id = "1";
            var itemRepository = FakeItemRepository.WithNoItems();
            var response = await CreateItem(itemRepository, new AddItemModel
                {Id = id, Category = "gaming", Retailer = "Amazon"});

            var allItems = await itemRepository.GetAll();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Single(allItems);
            Assert.NotNull(allItems.Select(item => item.Id == id));
        }
    }
}
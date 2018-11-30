using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PriceChat.Web.Models.Items;
using PriceChat.Web.Tests.Fakes;
using Xunit;

namespace PriceChat.Web.Tests.Controllers.ItemControllerTests
{
    public class PostAddTests : IClassFixture<ItemControllerFixture>
    {
        private readonly ItemControllerBuilder _builder;

        public PostAddTests(ItemControllerFixture fixture) 
            => _builder = fixture.Builder;

        [Fact]
        public async Task GivenAnInvalidItemModel_ViewIsReturnedWithErrorsAndItemIsNotSaved()
        {
            var itemModel = new ItemModel();
            var repository = new ItemRepositoryWithNoItems();

            var itemController = _builder.WithItemRepository(repository).Build();
            var result = Assert.IsType<ViewResult>(await itemController.Add(itemModel));
            var viewModel = result.Model as ItemModel;

            Assert.Null(result.ViewName);
            Assert.Equal(itemModel, viewModel);
            Assert.Empty(await repository.GetAll());
        }

        [Fact]
        public async Task GivenAValidItemModel_ItemIsAddedViaTheRepository()
        {
            var itemModel = new ItemModel {Id = "007", Category = "Gaming", Retailer = "Amazon"};
            var repository = new ItemRepositoryWithNoItems();

            var itemController = _builder.WithItemRepository(repository).Build();
            var result = Assert.IsType<ViewResult>(await itemController.Add(itemModel));
            Assert.Null(result.ViewName);

            var collection = await repository.GetAll();
            Assert.Single(collection);

            var itemFromDatabase = collection.First();
            Assert.Equal(itemModel.Id, itemFromDatabase.Id);
            Assert.Equal(itemModel.Category, itemFromDatabase.Category);
            Assert.Equal(itemModel.Retailer, itemFromDatabase.Retailer);
        }
    }
}
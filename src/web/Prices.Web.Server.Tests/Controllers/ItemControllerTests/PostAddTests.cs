using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Prices.Web.Server.Tests.Fakes;
using Prices.Web.Shared.Models.Items;
using Xunit;

namespace Prices.Web.Server.Tests.Controllers.ItemControllerTests
{
    public class PostAddTests : IClassFixture<ItemControllerFixture>
    {
        private readonly ItemControllerBuilder _builder;

        public PostAddTests(ItemControllerFixture fixture) 
            => _builder = fixture.Builder;

        [Fact]
        public async Task GivenAnInvalidItemModel_ViewIsReturnedWithErrorsAndItemIsNotSaved()
        {
            var itemModel = new AddItemModel();
            var repository = new ItemRepositoryWithNoItems();

            var itemController = _builder.WithItemRepository(repository).Build();
            var result = Assert.IsType<ViewResult>(await itemController.Add(itemModel));
            var viewModel = result.Model as AddItemModel;

            Assert.Null(result.ViewName);
            itemModel.Should().BeEquivalentTo(viewModel, m => m.Excluding(e => e.Errors));
            Assert.Empty(await repository.GetAll());
        }

        [Fact]
        public async Task GivenAnInvalidItemId_ErrorsContainsCorrectMessage()
        {
            var itemModel = new AddItemModel();
            var repository = new ItemRepositoryWithNoItems();

            var itemController = _builder.WithItemRepository(repository).Build();
            var result = Assert.IsType<ViewResult>(await itemController.Add(itemModel));
            var viewModel = result.Model as AddItemModel;

            Assert.Null(result.ViewName);
            itemModel.Should().BeEquivalentTo(viewModel, m => m.Excluding(e => e.Errors));
            Assert.Contains("Item Id is required", itemModel.Errors.Select(error => error.ErrorMessage));
        }

        [Fact]
        public async Task GivenAnInvalidItemCategory_ErrorsContainsCorrectMessage()
        {
            var itemModel = new AddItemModel();
            var repository = new ItemRepositoryWithNoItems();

            var itemController = _builder.WithItemRepository(repository).Build();
            var result = Assert.IsType<ViewResult>(await itemController.Add(itemModel));
            var viewModel = result.Model as AddItemModel;

            Assert.Null(result.ViewName);
            itemModel.Should().BeEquivalentTo(viewModel, m => m.Excluding(e => e.Errors));
            Assert.Contains("Item Category is required", itemModel.Errors.Select(error => error.ErrorMessage));
        }

        [Fact]
        public async Task GivenAnInvalidItemRetailer_ErrorsContainsCorrectMessage()
        {
            var itemModel = new AddItemModel();
            var repository = new ItemRepositoryWithNoItems();

            var itemController = _builder.WithItemRepository(repository).Build();
            var result = Assert.IsType<ViewResult>(await itemController.Add(itemModel));
            var viewModel = result.Model as AddItemModel;

            Assert.Null(result.ViewName);
            itemModel.Should().BeEquivalentTo(viewModel, m => m.Excluding(e => e.Errors));
            Assert.Contains("Item Retailer is required", itemModel.Errors.Select(error => error.ErrorMessage));
        }

        [Fact]
        public async Task GivenAnUnknownRetailer_ErrorsContainsCorrectMessage()
        {
            var itemModel = new AddItemModel{ Retailer = "Bobs business"};
            var repository = new ItemRepositoryWithNoItems();

            var itemController = _builder.WithItemRepository(repository).Build();
            var result = Assert.IsType<ViewResult>(await itemController.Add(itemModel));
            var viewModel = result.Model as AddItemModel;

            Assert.Null(result.ViewName);
            itemModel.Should().BeEquivalentTo(viewModel, m => m.Excluding(e => e.Errors));
            Assert.Contains("Please specify a known retailer", itemModel.Errors.Select(error => error.ErrorMessage));
        }

        [Fact]
        public async Task GivenAValidItemModel_ItemIsAddedViaTheRepository()
        {
            var itemModel = new AddItemModel {Id = "007", Category = "Gaming", Retailer = "Amazon"};
            var repository = new ItemRepositoryWithNoItems();

            var itemController = _builder.WithItemRepository(repository).Build();
            var result = Assert.IsType<ViewResult>(await itemController.Add(itemModel));
            Assert.Null(result.ViewName);

            var viewModel = Assert.IsType<AddItemModel>(result.Model);
            Assert.True(viewModel.Success);

            var collection = await repository.GetAll();
            Assert.Single(collection);

            var itemFromDatabase = collection.First();
            Assert.Equal(itemModel.Id, itemFromDatabase.Id);
            Assert.Equal(itemModel.Category, itemFromDatabase.Category);
            Assert.Equal(itemModel.Retailer, itemFromDatabase.Retailer);
        }
    }
}
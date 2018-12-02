using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PriceChat.Web.Models;
using PriceChat.Web.Models.Home;
using PriceChat.Web.Tests.Fakes;
using Xunit;

namespace PriceChat.Web.Tests.Controllers.HomeControllerTests
{
    public class IndexTests
    {
        public class GivenItemsInTheDatabase : IClassFixture<HomeControllerTestFixture>
        {
            private readonly HomeControllerTestBuilder _builder;

            public GivenItemsInTheDatabase(HomeControllerTestFixture fixture) 
                => _builder = fixture.Builder;

            [Fact]
            public async Task WhenDefaultPageIsRequested_ThenCorrectPageIsReturnedWithItems()
            {
                var homeController = _builder
                    .WithItemRepository(new ItemRepositoryWithItems())
                    .BuildController();
                var viewResult = await homeController.Index() as ViewResult;

                Assert.NotNull(viewResult);
                Assert.Null(viewResult.ViewName);

                var viewModel = viewResult.Model as List<Item>;
                Assert.NotNull(viewModel);
                Assert.Equal(ItemRepositoryWithItems.Items.Count, viewModel.Count);
            }
        }

        public class GivenNoItemExistInTheDatabase : IClassFixture<HomeControllerTestFixture>
        {
            private readonly HomeControllerTestBuilder _builder;

            public GivenNoItemExistInTheDatabase(HomeControllerTestFixture fixture) 
                => _builder = fixture.Builder;

            [Fact]
            public async Task WhenDefaultPageIsRequested_ThenCorrectPageIsReturnedWithEmptyList()
            {
                var homeController = _builder
                    .WithItemRepository(new ItemRepositoryWithNoItems())
                    .BuildController();
                var viewResult = await homeController.Index() as ViewResult;

                Assert.NotNull(viewResult);
                Assert.Null(viewResult.ViewName);

                var viewModel = viewResult.Model as List<Item>;
                Assert.NotNull(viewModel);
                Assert.Empty(viewModel);
            }
        }
    }
}
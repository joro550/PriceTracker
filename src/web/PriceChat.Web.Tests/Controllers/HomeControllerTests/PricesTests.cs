using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using PriceChat.Web.Data;
using PriceChat.Web.Tests.Fakes;
using Xunit;

namespace PriceChat.Web.Tests.Controllers.HomeControllerTests
{
    public class PricesTests
    {
        public class GivenItemsInTheDatabase : IClassFixture<HomeControllerTestFixture>
        {
            private readonly HomeControllerTestBuilder _builder;

            public GivenItemsInTheDatabase(HomeControllerTestFixture fixture) 
                => _builder = fixture.Builder;

            [Fact]
            public async Task WhenRequestingPricesView_ThenPricesAreInTheModel()
            {
                var fixture = new Fixture();
                var itemPrice = fixture
                    .Create<ItemPrice>();

                var repository = new ItemPriceRepositoryWithSpecifiedPrices(new List<ItemPrice> {itemPrice});
                var homeController = _builder
                    .WithItemPriceRepository(repository)
                    .BuildController();

                var viewResult = await homeController.Prices(itemPrice.PartitionKey) as ViewResult;
                Assert.NotNull(viewResult);
                Assert.Null(viewResult.ViewName);

                var viewModel = viewResult.Model as List<Models.Home.ItemPrice>;
                Assert.NotNull(viewModel);
                Assert.Single(viewModel);
                Assert.Equal(itemPrice.PartitionKey, viewModel[0].PartitionKey);
                Assert.Equal(itemPrice.Price, viewModel[0].Price);
            }
        }

        public class GivenNoPricesInTheDatabase : IClassFixture<HomeControllerTestFixture>
        {
            private readonly HomeControllerTestBuilder _builder;

            public GivenNoPricesInTheDatabase(HomeControllerTestFixture fixture) 
                => _builder = fixture.Builder;

            [Fact]
            public async Task WhenRequestingPricesView_ThenModelHasEmptyCollection()
            {
                var homeController = _builder
                    .WithItemPriceRepository(new ItemPriceRepositoryWithNoPrices())
                    .BuildController();
                
                var viewResult = await homeController.Prices(string.Empty) as ViewResult;
                Assert.NotNull(viewResult);
                Assert.Null(viewResult.ViewName);
                
                var viewModel = viewResult.Model as List<Models.Home.ItemPrice>;
                Assert.NotNull(viewModel);
                Assert.Empty(viewModel);
            }
        }
    }
}